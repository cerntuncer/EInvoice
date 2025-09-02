import os


class Settings:
    # Default model directory where you keep saved_pretrained outputs
    MODEL_DIR: str = os.getenv("MODEL_DIR", "/workspace/ai-service/models/model_tr_support2")
    DEVICE: str = os.getenv("DEVICE", "cpu")


settings = Settings()

