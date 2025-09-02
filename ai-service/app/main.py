from fastapi import FastAPI, HTTPException
from typing import List
import os

from .config import settings
from .model_loader import load_model, predict_texts
from .schemas import PredictRequest, PredictResponse, Prediction


app = FastAPI(title="AI Service - Turkish Support Classifier")


@app.on_event("startup")
def _startup_load_model() -> None:
    try:
        load_model(settings.MODEL_DIR, settings.DEVICE)
    except Exception as exc:
        # Fail fast with clear message so deployment surfaces configuration issue
        raise RuntimeError(f"Model could not be loaded from {settings.MODEL_DIR}: {exc}")


@app.get("/health")
def health() -> dict:
    return {"status": "ok"}


@app.get("/labels")
def labels() -> dict:
    bundle = load_model(settings.MODEL_DIR, settings.DEVICE)
    # config.id2label can have int keys after load
    id2label = {int(k): v for k, v in bundle.id2label.items()} if isinstance(next(iter(bundle.id2label.keys()), 0), str) else bundle.id2label
    ordered = [id2label[i] for i in sorted(id2label.keys())]
    return {"labels": ordered}


@app.post("/predict", response_model=PredictResponse)
def predict(req: PredictRequest) -> PredictResponse:
    texts: List[str] = []
    if req.text:
        texts = [req.text]
    elif req.texts:
        texts = req.texts
    else:
        raise HTTPException(status_code=400, detail="Provide 'text' or 'texts'")

    try:
        results = predict_texts(texts=texts, model_dir=settings.MODEL_DIR, device=settings.DEVICE, top_k=req.top_k)
        predictions = [Prediction(label=r[0]["label"], score=r[0]["score"]) if req.top_k == 1 else [Prediction(**p) for p in r] for r in results]
        # Normalize to list of Prediction for response_model; when top_k>1 flatten per item is list
        # To keep schema simple, return first prediction when top_k==1
        if req.top_k == 1:
            predictions = [predictions[i] for i in range(len(predictions))]
            return PredictResponse(predictions=predictions)
        else:
            # For top_k>1, pack as best-first list per item in a generic field
            # Keep compatibility by returning only the best label; include scores as metadata
            best_only = [p_list[0] for p_list in predictions]
            return PredictResponse(predictions=best_only)
    except FileNotFoundError as fnf:
        raise HTTPException(status_code=500, detail=str(fnf))
    except Exception as exc:
        raise HTTPException(status_code=500, detail=f"Inference failed: {exc}")

