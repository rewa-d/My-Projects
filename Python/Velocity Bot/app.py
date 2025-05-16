from flask import Flask, request, jsonify, render_template
import requests
import sqlite3

app = Flask(__name__)
OLLAMA_URL = "http://localhost:11434/api/chat"
MODEL_NAME = "gemma"
DB_PATH = "velocity_bot_log.db"
MAX_HISTORY = 10

# Read company context from file
def load_company_context(path="company_context.txt"):
    try:
        with open(path, "r", encoding="utf-8") as file:
            return file.read()
    except FileNotFoundError:
        return "You are a helpful assistant."

COMPANY_CONTEXT = load_company_context()
chat_history = [{"role": "system", "content": COMPANY_CONTEXT}]

# Create DB table
conn = sqlite3.connect(DB_PATH)
cursor = conn.cursor()
cursor.execute('''
CREATE TABLE IF NOT EXISTS chat_log (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_input TEXT,
    bot_response TEXT
)
''')
conn.commit()
conn.close()

@app.route("/")
def index():
    return render_template("index.html")

@app.route("/chat", methods=["POST"])
def chat():
    user_input = request.json.get("message", "")
    chat_history.append({"role": "user", "content": user_input})
    pruned_history = [chat_history[0]] + chat_history[-MAX_HISTORY:]

    response = requests.post(OLLAMA_URL, json={
        "model": MODEL_NAME,
        "messages": pruned_history,
        "stream": False,
        "options": {
            "num_predict": 120
        }
    }, timeout=240)

    bot_reply = response.json()["message"]["content"].strip()
    chat_history.append({"role": "assistant", "content": bot_reply})

    conn = sqlite3.connect(DB_PATH)
    cursor = conn.cursor()
    cursor.execute("INSERT INTO chat_log (user_input, bot_response) VALUES (?, ?)", (user_input, bot_reply))
    conn.commit()
    conn.close()

    return jsonify({"reply": bot_reply})

if __name__ == "__main__":
    app.run(debug=True)
