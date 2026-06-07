# WellYouth Development Context

## Project Overview
WellYouth is a youth-focused health tracking MVC application built with ASP.NET Core, EF Core, and Identity.

## Current Status
- **Phase:** Phase 8 Complete (Premium AI Upgrade)
- **Completed:** 
    - Task 1: Foundation & Infrastructure (Project setup, DB context, Entities, Migrations, Service Stubs)
    - Task 2: Authentication & User Management (AccountController, ViewModels, Register/Login Views, Identity integration)
    - Task 3: Core Layout & Navigation (Responsive Sidebar, Floating AI button, Dashboard, Layout overhaul)
    - Task 4: Health Tracking & Scoring (Habit Logging, Point Calculations, Recommendation Service, Profile View)
    - Task 5: Content & Directory Modules (Articles Module, Specialist Directory, Database Seeder)
    - Task 6: Engagement Modules (Community Groups, Postings, Wellness Activities/Games)
    - Task 7: AI Assistant Foundation (Chat Service, Sidebar, Basic UI)
    - Task 8: Real AI Integration & Premium UX (Gemini LLM, AJAX messaging, Chat Deletion, Typewriter Effect)

## Recent Session Upgrades (Phase 8)
- **Engine**: Replaced rule-based stubs with a real Gemini 1.5 Flash integration.
- **UX**: Implemented a "live" chat experience using AJAX/JSON communication.
    - Added a "Thinking..." indicator with bouncing dots.
    - Added a high-speed (5ms) Typewriter effect for AI responses.
    - Added support for structured formatting (bolding `**`, newlines `\n`, and bullet points).
- **Functionality**: Added a full "Delete Chat" feature with database cleanup and confirmation prompts.
- **Security**: Moved API credentials to a `.env` file (excluded from Git) using `DotNetEnv`.
- **UI Styling**: Refined chat bubble aesthetics for better light-mode contrast and theme consistency.

## Architectural Decisions
- **Database:** SQL Server LocalDB.
- **ORM:** Entity Framework Core with Global Cascade Delete set to `Restrict`.
- **Auth:** ASP.NET Core Identity using `ApplicationUser`.
- **Services:** Scoped services for business logic.
- **AI Engine:** Google Gemini (Gemini 3.5 Flash) via `Mscc.GenerativeAI`.
- **Configuration:** Environment-based secrets using `.env` and `DotNetEnv`.
- **UI:** Responsive sidebar/topbar layout using Bootstrap 5 and FontAwesome 6; AJAX-based async chat with typewriter effect and Markdown-lite formatting.

## Final Summary
The WellYouth platform is a high-fidelity wellness application featuring:
- Secure User Authentication and Profiles.
- Daily Health Habit Tracking with automated point scoring.
- Personalized health suggestions based on user activity.
- An expert-reviewed health article library.
- A verified specialist directory with contact request features.
- Community groups for social support and discussion.
- A guided breathing activity for immediate wellness engagement.
- A premium AI Health Assistant powered by Gemini 3.5 Flash, featuring persistent history, context-aware wellness coaching (habits & score awareness), and a modern, refresh-free "live" chat interface.

## Notes
- Database is pre-seeded with categories, articles, specialists, and community groups.
- The project follows a clean MVC + Service Layer architecture.
- API Key must be populated in the local `.env` file for the Assistant to function.
