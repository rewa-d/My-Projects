# 🤖 Velocity Bot – AI Chat Assistant (Customizable)

Velocity Bot is a Flask-based AI chatbot powered by a local Ollama model (e.g., Gemma, Mistral).  
It is designed to help customers interact with your business through a web chat interface, using context from a text file.

---

## 🚀 Features

- 🔧 Works with any company – just update `company_context.txt`
- 🧠 Uses local LLMs (via Ollama) – no external API costs
- 💬 Responsive web-based chat widget
- 💾 Stores chat logs in SQLite

---

## 📁 Project Structure

```
velocity-bot/
├── app.py                   # Flask backend
├── company_context.txt      # Context used to define your company info
├── templates/
│   └── index.html           # Web chat interface
├── static/
│   ├── style.css            # UI styling
│   └── script.js            # Frontend logic (fetch/send chat)
└── velocity_bot_log.db      # Chat history database (auto-generated)
```

---

## ✍️ Customizing the Bot for Any Company

To make the bot work for your business:

1. Open `company_context.txt`
2. Replace the default text with your own:
   ```text
   You are [Your Bot Name], an AI assistant for [Your Company Name].

   Office: ...
   Services: ...
   Achievements: ...
   ```

3. Save the file — no code changes needed!

---

## ⚙️ Installation

### 1. Install Python (3.10+ recommended)

### 2. Clone the repository

```bash
git clone https://github.com/your-username/velocity-bot.git
cd velocity-bot
```

### 3. Set up a virtual environment

```bash
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
```

### 4. Install dependencies

Install Flask and Requests using:

```bash
pip install Flask requests
```
OR

```bash
pip install Flask==2.3.3 requests==2.31.0
```

---

### 5. Install Ollama and a Local Model

Install [Ollama](https://ollama.com/download)

First download the "Gemma Model" using:
```bash
ollama pull gemma  
```

Then run the model using:
```bash
ollama run gemma  
```

---

## ▶️ Running the App

Start the Flask server:

```bash
python app.py
```

Visit: [http://127.0.0.1:5000](http://127.0.0.1:5000)

---

## 🧠 How It Works

- On startup, the app loads `company_context.txt`
- The content is injected into the chat as the **system prompt**
- User messages are sent to the local LLM (via Ollama API)
- The bot responds using that context
- All chats are logged in an SQLite database

---

## 📥 Example Context (company_context.txt)

```text
You are TravelBot, an AI assistant for MakeMyTrip.

You help users with:
- Flight bookings
- Hotel reservations
- Trip packages
- Customer support

Office: Gurugram, India  
Business Hours: 24/7 support  
```

---

## 👨‍💻 Author

**Rewa Dambal**  
GitHub: [https://github.com/rewa-d](https://github.com/rewa-d)
