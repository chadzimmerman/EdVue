# 📱 WGU Student Term Tracker (.NET MAUI)

This is a cross-platform mobile application built using the .NET MAUI framework as part of the WGU C971: **Mobile Application Development Using C#** course. The app allows WGU students to manage and track their academic progress across terms, courses, and assessments. This application demonstrates end-to-end mobile app design, SQLite database integration, user notification scheduling, and professional-level mobile development practices.

---

## 🚀 Features

- **Multi-screen architecture** using .NET MAUI and MVVM pattern.
- **SQLite local storage** via `SQLite-net` ORM for persistent offline data management.
- **CRUD operations** for:
  - Academic Terms
  - Courses (up to six per term)
  - Assessments (2 per course: Performance & Objective)
- **Custom notifications** for assessment and course dates using `Plugin.LocalNotification`.
- **Date pickers and status pickers** using native `.NET MAUI` controls.
- **Note sharing** via `Share` plugin (SMS, Email, etc.).
- **Clean UI/UX** with a tab-based navigation interface and modern styling.
- **Validation** for course instructor email/phone and empty fields.

---
<img width="358" height="757" alt="Screenshot 2026-02-28 at 3 41 46 PM" src="https://github.com/user-attachments/assets/f9477a4a-7564-40fc-a36a-5e4ef76b5f70" />
<img width="358" height="757" alt="Screenshot 2026-02-28 at 3 42 29 PM" src="https://github.com/user-attachments/assets/735f7bfa-07c5-4d67-a947-a1eb842a0c57" />
<img width="358" height="757" alt="Screenshot 2026-02-28 at 3 42 57 PM" src="https://github.com/user-attachments/assets/40227bea-717d-4381-b78d-980d466b24a5" />

## 📚 Application Structure

```plaintext
├── Models
│   ├── Term.cs
│   ├── Course.cs
│   └── Assessment.cs
├── Views
│   ├── TermListPage.xaml
│   ├── CourseListPage.xaml
│   ├── CourseDetailPage.xaml
│   └── AssessmentPage.xaml
├── ViewModels
│   ├── TermViewModel.cs
│   ├── CourseViewModel.cs
│   └── AssessmentViewModel.cs
├── Servicesgit 
│   └── DatabaseService.cs
├── App.xaml / MainPage.xaml
