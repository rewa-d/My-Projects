from flask import Flask, render_template, request, jsonify
import pandas as pd
import joblib
from sklearn.preprocessing import StandardScaler

# Load the trained model and scaler
model = joblib.load('lung_cancer_model.pkl')
scaler = joblib.load('scaler.pkl')

# Initialize Flask app
app = Flask(__name__)


# Function to encode input data before making predictions
def encode_input_data(input_data):
    input_data['YELLOW_FINGERS'] = input_data['YELLOW_FINGERS'].map({'Yes': 1, 'No': 0})
    input_data['ANXIETY'] = input_data['ANXIETY'].map({'Yes': 1, 'No': 0})
    input_data['PEER_PRESSURE'] = input_data['PEER_PRESSURE'].map({'Yes': 1, 'No': 0})
    input_data['CHRONIC DISEASE'] = input_data['CHRONIC DISEASE'].map({'Yes': 1, 'No': 0})
    input_data['FATIGUE'] = input_data['FATIGUE'].map({'Yes': 1, 'No': 0})
    input_data['ALLERGY'] = input_data['ALLERGY'].map({'Yes': 1, 'No': 0})
    input_data['WHEEZING'] = input_data['WHEEZING'].map({'Yes': 1, 'No': 0})
    input_data['ALCOHOL CONSUMING'] = input_data['ALCOHOL CONSUMING'].map({'Yes': 1, 'No': 0})
    input_data['COUGHING'] = input_data['COUGHING'].map({'Yes': 1, 'No': 0})
    input_data['SWALLOWING DIFFICULTY'] = input_data['SWALLOWING DIFFICULTY'].map({'Yes': 1, 'No': 0})
    input_data['CHEST PAIN'] = input_data['CHEST PAIN'].map({'Yes': 1, 'No': 0})
    return input_data


# Function to make predictions using the trained SVM model
def predict_lung_cancer(input_data):
    # Apply encoding to input data
    input_data_encoded = encode_input_data(input_data)

    # Standardize the input data using the same scaler used during training
    input_data_scaled = scaler.transform(input_data_encoded)

    # Make prediction using the trained SVM model
    prediction = model.predict(input_data_scaled)

    return prediction


# Home route to display the input form
@app.route('/')
def home():
    return render_template('index.html')


# Prediction route to handle the form submission
@app.route('/predict', methods=['POST'])
def predict():
    yellow_fingers = request.form['YELLOW_FINGERS']
    anxiety = request.form['ANXIETY']
    peer_pressure = request.form['PEER_PRESSURE']
    chronic_disease = request.form['CHRONIC DISEASE']
    fatigue = request.form['FATIGUE ']
    allergy = request.form['ALLERGY ']
    wheezing = request.form['WHEEZING']
    alcohol_consuming = request.form['ALCOHOL CONSUMING']
    coughing = request.form['COUGHING']
    swallowing_difficulty = request.form['SWALLOWING DIFFICULTY']
    chest_pain = request.form['CHEST PAIN']

    # Prepare input data for prediction
    input_data = pd.DataFrame([[yellow_fingers, anxiety, peer_pressure, chronic_disease,
                                fatigue, allergy, wheezing, alcohol_consuming, coughing,
                             swallowing_difficulty, chest_pain]],
                              columns=['YELLOW_FINGERS', 'ANXIETY', 'PEER_PRESSURE',
                                       'CHRONIC DISEASE', 'FATIGUE', 'ALLERGY', 'WHEEZING', 'ALCOHOL CONSUMING',
                                       'COUGHING', 'SWALLOWING DIFFICULTY', 'CHEST PAIN'])

    # Make prediction
    result = predict_lung_cancer(input_data)

    # Return the result to the user
    if result == 1:
        return render_template('index.html', prediction_text="The model predicts that you have **Lung Cancer**.")
    else:
        return render_template('index.html', prediction_text="The model predicts that you **do not have Lung Cancer**.")


if __name__ == "__main__":
    app.run(debug=True)
