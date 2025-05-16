const chatWidget = document.getElementById("chat-widget");
const chatMessages = document.getElementById("chat-messages");
const chatInput = document.getElementById("chat-input");

document.getElementById("chat-button").onclick = toggleChat;

function toggleChat() {
  chatWidget.style.display = chatWidget.style.display === "none" ? "flex" : "none";
}

function appendMessage(sender, text) {
  const wrapper = document.createElement("div");
  wrapper.className = sender === "You" ? "user-msg" : "bot-msg";
  wrapper.textContent = text;
  chatMessages.appendChild(wrapper);
  chatMessages.scrollTop = chatMessages.scrollHeight;
}

function showTyping() {
  const typingDiv = document.createElement("div");
  typingDiv.className = "typing-indicator";
  typingDiv.id = "typing-indicator";
  typingDiv.textContent = "Velocity Bot is typing...";
  chatMessages.appendChild(typingDiv);
  chatMessages.scrollTop = chatMessages.scrollHeight;
}

function removeTyping() {
  const typing = document.getElementById("typing-indicator");
  if (typing) typing.remove();
}

function sendMessage() {
  const msg = chatInput.value.trim();
  if (!msg) return;
  appendMessage("You", msg);
  chatInput.value = "";

  showTyping();

  fetch("/chat", {
    method: "POST",
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ message: msg })
  })
  .then(res => res.json())
  .then(data => {
    removeTyping();
    appendMessage("Velocity Bot", data.reply);
  })
  .catch(() => {
    removeTyping();
    appendMessage("Velocity Bot", "Oops! Server not responding.");
  });
}

chatInput.addEventListener("keypress", function (e) {
  if (e.key === "Enter") sendMessage();
});
