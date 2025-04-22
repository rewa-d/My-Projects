function calculateCutoff() {
  const m = parseFloat(document.getElementById("maths").value) || 0;
  const p = parseFloat(document.getElementById("physics").value) || 0;
  const c = parseFloat(document.getElementById("chemistry").value) || 0;

  const cutoff = (m / 2 + p / 4 + c / 4).toFixed(2);
  document.getElementById("cutoffResult").innerText = `Cut-Off Mark: ${cutoff}`;
}

function calculateGrade() {
  const marks = parseFloat(document.getElementById("gradeMarks").value) || 0;
  let grade = "";

  if (marks >= 90) grade = "A+";
  else if (marks >= 80) grade = "A";
  else if (marks >= 70) grade = "B+";
  else if (marks >= 60) grade = "B";
  else if (marks >= 50) grade = "C";
  else grade = "Fail";

  document.getElementById("gradeResult").innerText = `Grade: ${grade}`;
}

function calculateCGPA() {
  const input = document.getElementById("cgpaInput").value;
  const gpas = input
    .split(",")
    .map((x) => parseFloat(x.trim()))
    .filter((x) => !isNaN(x));

  if (gpas.length === 0) {
    document.getElementById("cgpaResult").innerText =
      "Please enter valid GPA values.";
    return;
  }

  const sum = gpas.reduce((a, b) => a + b, 0);
  const avg = (sum / gpas.length).toFixed(2);

  document.getElementById("cgpaResult").innerText = `Calculated CGPA: ${avg}`;
}
