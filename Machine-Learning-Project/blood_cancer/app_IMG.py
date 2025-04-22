import streamlit as st
import numpy as np
import cv2
from tensorflow.keras.models import load_model
from tensorflow.keras.preprocessing import image

#loading the trained model
model_path = "blood_cancer_cnn_model.keras"
cnn_model = load_model(model_path)

#defining the category labels
categories = ["Basophil", "Erythroblast", "Monocyte", "Myeloblast", "Seg Neutrophil"]  # Adjust based on your dataset

# ✅ Function to preprocess uploaded image
def preprocess_image(uploaded_file):
    file_bytes = np.asarray(bytearray(uploaded_file.read()), dtype=np.uint8)
    img = cv2.imdecode(file_bytes, cv2.IMREAD_COLOR)  # Read image
    img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)  # Convert to RGB
    img = cv2.resize(img, (224, 224))  # Resize for the CNN model
    img = img / 255.0  # Normalize pixel values
    img = np.expand_dims(img, axis=0)  # Add batch dimension
    return img

# ✅ Streamlit UI
st.title("🔬 Blood Cancer Cell Classifier")
st.write("Upload a blood cell image to classify its type.")

# Upload file widget
uploaded_file = st.file_uploader("Choose an image...", type=["jpg", "png", "jpeg"])

# Process the uploaded image
if uploaded_file is not None:
    st.image(uploaded_file, caption="Uploaded Image", use_column_width=True)
    st.write("🕵️‍♂️ Processing...")

    # Preprocess image
    img = preprocess_image(uploaded_file)

    # Predict
    prediction = cnn_model.predict(img)
    predicted_class = np.argmax(prediction)  # Class index
    predicted_label = categories[predicted_class]  # Get class name

    # Display results
    st.write(f"✅ **Predicted Class:** {predicted_label}")
    st.write("📊 **Class Probabilities:**")

    # Show probabilities for all categories
    for i, category in enumerate(categories):
        st.write(f"{category}: {prediction[0][i]*100:.2f}%")

