from flask import Flask, jsonify, request
import pickle

app = Flask(__name__)

# Load model, vectorizer, and trivia dataset from the pickle file
with open("trivia_ai_model.pkl", "rb") as file:
    model, vectorizer, trivia_data = pickle.load(file)

# Global complexity level tracker for the single user
current_complexity = 1  # Start at complexity 1

@app.route('/get_trivia', methods=['GET'])
def get_trivia():
    global current_complexity
    try:
        print("current complexity", current_complexity)
        # Filter questions by current complexity
        try:
            filtered_questions = trivia_data[trivia_data['Complexity'] == current_complexity]
        except Exception as e:
            print(e)
        
        if filtered_questions.empty:
            return jsonify({'error': f'No questions found for complexity level {current_complexity}'}), 404

        # Pick a random question
        trivia_question = filtered_questions.sample(1).iloc[0]
       
        print(trivia_question)
        return jsonify({
            'questionId': int(trivia_question['QuestionId']),
            'question': str(trivia_question['Question']),
            'correct_answer': str(trivia_question['Answer']),
            'complexity': int(trivia_question['Complexity'])  
        })

    except Exception as e:
        return jsonify({'error': str(e)})

@app.route('/submit_answer', methods=['POST'])
def submit_answer():
    global current_complexity
    try:
        data = request.get_json()
        question_id = data.get('question_id')
        is_correct = data.get('is_correct')

        if question_id is None or is_correct is None:
            return '', 400  # Bad request if data is missing

        # Only update complexity if the answer was correct
        if is_correct:
            current_complexity = min(current_complexity + 1, 3)

        return '', 200  # Success, no content

    except Exception as e:
        return '', 500  # Internal server error


if __name__ == "__main__":
    app.run(debug=True)
