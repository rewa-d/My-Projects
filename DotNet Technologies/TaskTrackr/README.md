# TaskTrackr

## Table of Contents

- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Setup and Installation](#setup-and-installation)
- [Building the Project](#building-the-project)
- [Running the Project](#running-the-project)

---

## Introduction

This is **TaskTrackr**, a typical web application used for task tracking, built using the .NET framework. The project demonstrates the use of basic Frontend designing using .NET components, CSS and JavaScript components, database management using MySQL and the Backend coding using C#.

---

## Prerequisites

Before running this project, ensure you have the following installed:

1. **Visual Studio 2019/2022** (or higher):

   - You can download the latest version of Visual Studio [here](https://visualstudio.microsoft.com/).

2. **.NET SDK**:

   - This project requires the .NET SDK version [8.0.403].
   - You can download the required .NET SDK from [here](https://dotnet.microsoft.com/download/dotnet).

3. **Target Framework**:

   - This project requires the .NET Framework 4.8.1.

4. **NuGet Packages**:

   - All required NuGet packages will be restored automatically when you build the project in Visual Studio.

---

## Setup and Installation

### Step 1: Clone the repository

To get started, first clone this repository to your local machine:

```bash
git clone https://github.com/rewa-d/My-Projects.git
```

## Step 2: Open the project in Visual Studio

1. Launch **Visual Studio**.
2. In the Visual Studio welcome screen, click on **Open a project or solution**.
3. Navigate to the cloned repository folder and select the `.sln` file to load the solution. (TaskTrackr_Project.sln)

---

## Step 3: Install dependencies

When the project is opened for the first time, Visual Studio will automatically restore all necessary NuGet packages.

If the packages do not restore automatically, you can restore them manually:

1. Go to **Tools** > **NuGet Package Manager** > **Manage NuGet Packages for Solution**.
2. Click **Restore** to install all required packages.

---

## Building the Project

To build the project:

1. Select **Build** > **Build Solution** in the top menu or press `Ctrl + Shift + B`.
2. The build output will appear in the **Output** window, indicating whether the build was successful.

---

## Running the Project

### Step 1: Set up the configuration

1. In the **Solution Explorer**, right-click on the project and select **Set as StartUp Project**.
2. Ensure the **Solution Configuration** is set to **Debug** and the **Solution Platform** is set to **Any CPU** (or another appropriate platform).

### Step 2: Run the application

1. Select the Home.aspx.
2. Press `F5` or click the **Start** button in Visual Studio to build and run the project.
3. The application will start running, and depending on the type of project, you will see the output:
   - **Web App**: A browser window will open with the application running at `http://localhost:xxxx`.

---
