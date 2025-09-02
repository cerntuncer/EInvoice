from functools import lru_cache
from typing import Dict, List, Tuple
import os

import torch
from transformers import AutoModelForSequenceClassification, AutoTokenizer


class ModelBundle:
    def __init__(self, model, tokenizer, id2label: Dict, label2id: Dict, device: str):
        self.model = model
        self.tokenizer = tokenizer
        self.id2label = id2label
        self.label2id = label2id
        self.device = device


def _resolve_device(device: str) -> str:
    if device and device.lower() == "cpu":
        return "cpu"
    if torch.cuda.is_available():
        return "cuda"
    return "cpu"


@lru_cache(maxsize=1)
def load_model(model_dir: str, device: str = "cpu") -> ModelBundle:
    model_dir = os.path.abspath(model_dir)
    if not os.path.isdir(model_dir):
        raise FileNotFoundError(f"Model directory not found: {model_dir}")

    resolved_device = _resolve_device(device)

    tokenizer = AutoTokenizer.from_pretrained(model_dir)
    model = AutoModelForSequenceClassification.from_pretrained(model_dir)
    model.to(resolved_device)
    model.eval()

    # HF ensures these exist in config when trained appropriately
    config = model.config
    id2label = getattr(config, "id2label", {}) or {}
    label2id = getattr(config, "label2id", {}) or {}

    # Normalize keys to int for id2label
    normalized_id2label = {}
    for k, v in id2label.items():
        try:
            normalized_id2label[int(k)] = v
        except Exception:
            normalized_id2label[k] = v

    return ModelBundle(model=model, tokenizer=tokenizer, id2label=normalized_id2label, label2id=label2id, device=resolved_device)


def predict_texts(texts: List[str], model_dir: str, device: str = "cpu", top_k: int = 1) -> List[List[Dict[str, float]]]:
    bundle = load_model(model_dir, device)
    tokenizer = bundle.tokenizer
    model = bundle.model

    enc = tokenizer(texts, padding=True, truncation=True, max_length=128, return_tensors="pt")
    enc = {k: v.to(bundle.device) for k, v in enc.items()}

    with torch.no_grad():
        logits = model(**enc).logits
        probs = torch.softmax(logits, dim=-1)

    top_k = max(1, int(top_k))
    top_probs, top_indices = torch.topk(probs, k=min(top_k, probs.shape[-1]), dim=-1)

    results: List[List[Dict[str, float]]] = []
    for i in range(top_indices.shape[0]):
        preds_for_sample: List[Dict[str, float]] = []
        for j in range(top_indices.shape[1]):
            idx = int(top_indices[i, j].item())
            score = float(top_probs[i, j].item())
            label = bundle.id2label.get(idx, str(idx))
            preds_for_sample.append({"label": label, "score": score})
        results.append(preds_for_sample)

    return results

