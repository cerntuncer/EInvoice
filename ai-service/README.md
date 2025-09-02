# AI Service (Turkish Support Classifier)

FastAPI microservice that loads a HuggingFace AutoModelForSequenceClassification and exposes /predict, /labels, and /health.

Model directory
- Place your exported save_pretrained folder under: /workspace/ai-service/models/model_tr_support2
- It should contain files like config.json, pytorch_model.bin (or model.safetensors), tokenizer.json, vocab.txt, etc.
- Or set an absolute path via env: MODEL_DIR=/abs/path/to/model_tr_support2

Run locally
1) cd /workspace/ai-service
2) python -m venv .venv && source .venv/bin/activate
3) pip install -r requirements.txt
4) export MODEL_DIR=/workspace/ai-service/models/model_tr_support2
5) uvicorn app.main:app --host 0.0.0.0 --port 8000

Docker
- docker build -t ai-service:latest .
- docker run -p 8000:8000 -e DEVICE=cpu -e MODEL_DIR=/app/models/model_tr_support2 \
  -v /workspace/ai-service/models/model_tr_support2:/app/models/model_tr_support2 \
  ai-service:latest

API
- GET /health
- GET /labels
- POST /predict

Request body example
{
  "text": "Merhaba, faturam neden kesilmedi?",
  "top_k": 1
}

Response
{
  "predictions": [
    {"label": "fatura_sorunu", "score": 0.92}
  ]
}

Batch example
{
  "texts": ["...", "..."],
  "top_k": 1
}

Notes
- Large model files are git-ignored.
- DEVICE defaults to cpu; if CUDA is available, it will be used automatically.