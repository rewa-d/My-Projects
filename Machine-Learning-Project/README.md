# 🧠 Machine Learning Projects – Cancer Detection & Classification

This repository contains four ML-based projects targeting the detection and classification of cancer using both patient data and cell image classification. It includes Jupyter Notebook implementations and two web-based applications for real-time prediction using Flask and Streamlit.

---

## 📁 Projects Included

### 🩸 Blood Cancer Classification (Notebook)

- **File**: `BloodCancer.ipynb`
- **Goal**: Identify blood cancer cell types from image data using CNN.
- **Highlights**:
  - Image preprocessing
  - Model training and evaluation
  - Classification of cells like Basophil, Monocyte, etc.

### 🌬️ Lung Cancer Prediction (Notebook)

- **File**: `LungCancer.ipynb`
- **Goal**: Predict the presence of lung cancer from survey data.
- **Highlights**:
  - Label encoding, scaling
  - Multiple classification models (e.g., KNN, SVM)
  - Accuracy & confusion matrix

---

## 🌐 Web Applications

### 📊 Lung Cancer Form-based App (Flask)

- **File**: `app_CSV.py`
- **Functionality**:
  - Uses form inputs (e.g., Smoking, Chest Pain) for prediction
  - Model and scaler loaded from `.pkl` files
  - Returns prediction in browser UI

### 🖼️ Blood Cell Image Classifier (Streamlit)

- **File**: `app_IMG.py`
- **Functionality**:
  - Upload an image of a blood cell
  - Uses a pre-trained CNN to predict the type of cell
  - Outputs the predicted class with probabilities

---

## Project Structure

```
├── blood_cancer/
│   ├── BloodCancer.ipynb
│   ├── app_IMG.py
│   ├── model/
│   | ├── blood_cancer_cnn_model.keras
│   └── Blood_Cancer_Dataset/
├── lung_cancer/
│   ├── LungCancer.ipynb
│   ├── app_CSV.py
│   ├── model/
│   | ├── lung_cancer_model.pkl
│   | ├── scaler.pkl
│   ├── survey_lung_cancer.csv
│   ├── templates/
│   | ├── index.html
└── README.md
```

---

## 🧰 Tools & Libraries Used

- Python 3.x
- Pandas, NumPy
- Scikit-learn
- TensorFlow / Keras
- OpenCV
- Matplotlib, Seaborn
- Flask
- Streamlit
- Jupyter Notebook

---

## 🚀 How to Run

### Notebooks

```bash
jupyter notebook
```

Open:

- `BloodCancer.ipynb`
- `LungCancer.ipynb`

### Flask App (CSV-Based)

```bash
pip install flask pandas joblib scikit-learn
python app_CSV.py
```

Visit `http://127.0.0.1:5000` in your browser.

### Streamlit App (Image-Based)

```bash
pip install streamlit opencv-python tensorflow
streamlit run app_IMG_GitHubReady.py
```

---

## 📜 License

Licensed under the [MIT License](https://opensource.org/licenses/MIT)

---

## ✍️ Author

**[Rewa Dambal]**  
GitHub: (https://github.com/rewa-d)
