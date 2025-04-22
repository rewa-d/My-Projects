import pandas as pd
import pickle
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.neighbors import NearestNeighbors

# Load CSV
trivia_data = pd.read_csv(r"C:\Users\vedes\Documents\Rewa\Final Year\trivia_dataset.csv")
df = pd.DataFrame(trivia_data)

# Combine question with numeric complexity as string
df["QuestionWithComplexity"] = df["Question"].astype(str) + " Complexity: " + df["Complexity"].astype(str)

# TF-IDF vectorization
vectorizer = TfidfVectorizer()
X = vectorizer.fit_transform(df["QuestionWithComplexity"])

# Train nearest neighbors
model = NearestNeighbors(n_neighbors=1, metric="cosine")
model.fit(X)

# Save model
with open("trivia_ai_model.pkl", "wb") as file:
    pickle.dump((model, vectorizer, df), file)

print("✅ Model trained using textual complexity and saved.")
