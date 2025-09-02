from typing import List, Optional, Union
from pydantic import BaseModel, Field


class PredictRequest(BaseModel):
    text: Optional[str] = Field(default=None, description="Single text for prediction")
    texts: Optional[List[str]] = Field(default=None, description="Batch texts for prediction")
    top_k: int = Field(default=1, ge=1, le=10, description="Top-K labels to return")


class Prediction(BaseModel):
    label: str
    score: float


class PredictResponse(BaseModel):
    predictions: List[Prediction]

