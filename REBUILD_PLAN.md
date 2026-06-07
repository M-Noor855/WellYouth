# WellYouth Fresh Build Plan

## Purpose
This document is the main plan for building WellYouth from scratch in a clean folder.

It assumes the folder only keeps:

- The original project description PDF.
- This `REBUILD_PLAN.md` file.

The new project should be created cleanly with `dotnet new mvc`, then built step by step using this plan.

## Product Goal
WellYouth is a youth-focused health tracking website. It helps young people improve physical and mental health through simple daily tracking, profile statistics, articles, community support, activities, specialist access, and AI-guided suggestions.

The final website should include:

- A home dashboard that explains and links to all main sections.
- A profile page with health statistics, progress, habits, score, and suggestions.
- A community page with groups, posts, and clear safety guidelines.
- A specialist directory for psychiatry, gynecology, nutrition, and similar support.
- A short reviewed articles section.
- An AI assistant available from the side menu and a floating button.
- Simple interactive wellness activities or games.
- A monthly scoring or competition system.

## Main Quality Target
Build the project as a complete, maintainable MVC application from the beginning.

That means:

- Use a clean MVC structure from the beginning.
- Store important data in the database instead of hardcoding it in controllers.
- Separate database entities from view models.
- Keep controllers small.
- Put business logic in services.
- Use real database relationships.
- Add validation and safety rules early.
- Build mobile navigation correctly from the start.
- Keep health advice careful and general.
- Make future AI integration easy by hiding it behind a service interface.

## Recommended Stack
Use:

- ASP.NET Core MVC.
- Razor views.
- Entity Framework Core.
- SQL Server LocalDB for local development.
- ASP.NET Core Identity for accounts.
- Bootstrap for layout utilities.
- Custom CSS for the final visual style.
- JavaScript only for needed interactions.

Version rule:

- Use package versions that match the chosen .NET SDK.
- Avoid mixing preview packages with stable packages.
- If the teacher does not require a specific .NET version, use a stable SDK installed on the machine.

## Create The New Project
From the root folder:

```powershell
dotnet new mvc -n WellYouth
cd WellYouth
```

Add packages:

```powershell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

Build immediately:

```powershell
dotnet restore
dotnet build
```

Run the project:

```powershell
dotnet run
```

## Target Folder Structure
After creating the new MVC project, organize it like this:

```text
FinalProjectWeb/
  Mohammednour Final Project description.pdf
  REBUILD_PLAN.md
  WellYouth/
    Controllers/
    Data/
    Migrations/
    Models/
      Entities/
      ViewModels/
        Account/
        Profile/
        Articles/
        Community/
        Specialists/
        Assistant/
        Activities/
    Services/
      Interfaces/
      Implementations/
    Views/
      Account/
      Home/
      Profile/
      Habits/
      Articles/
      Community/
      Specialists/
      Assistant/
      Activities/
      Shared/
    wwwroot/
      css/
      js/
      images/
    Program.cs
    appsettings.json
    appsettings.Development.json
    WellYouth.csproj
```

## MVC Architecture
MVC should be the core shape of the project.

### Models
Models should be split into two types:

- Entity models: classes stored in the database.
- View models: classes shaped for a specific page or form.

Do not use one model for everything. For example, an `Article` database entity can include fields like `Body`, `Status`, and `ReviewedBy`, while an `ArticleListItemViewModel` only needs the fields displayed on the article list page.

### Entity Models
Create entity classes in `Models/Entities/`.

Recommended entities:

```text
ApplicationUser.cs
HealthHabit.cs
HealthHabitLog.cs
HealthStat.cs
HealthScoreEntry.cs
Article.cs
ArticleCategory.cs
Specialist.cs
SpecialistContactRequest.cs
CommunityGroup.cs
CommunityGroupMember.cs
CommunityPost.cs
AssistantConversation.cs
AssistantMessage.cs
WellnessActivity.cs
UserActivityLog.cs
```

Entity purpose:

- `ApplicationUser`: extends Identity user with profile fields.
- `HealthHabit`: habit created by a user, such as water, sleep, walking, or meditation.
- `HealthHabitLog`: one daily completion record for a habit.
- `HealthStat`: tracked values such as sleep hours, water intake, mood, or steps.
- `HealthScoreEntry`: score changes over time.
- `Article`: reviewed article content.
- `ArticleCategory`: article grouping.
- `Specialist`: professional contact information.
- `SpecialistContactRequest`: user request to contact a specialist.
- `CommunityGroup`: support group.
- `CommunityGroupMember`: relation between users and groups.
- `CommunityPost`: post inside a group.
- `AssistantConversation`: chat session.
- `AssistantMessage`: message in a chat session.
- `WellnessActivity`: interactive activity or game.
- `UserActivityLog`: completed activity record for scoring.

### View Models
Create view models in `Models/ViewModels/`.

Recommended view models:

```text
Account/LoginViewModel.cs
Account/RegisterViewModel.cs
Profile/ProfileDashboardViewModel.cs
Profile/CreateHabitViewModel.cs
Profile/UpdateHabitLogViewModel.cs
Articles/ArticleListViewModel.cs
Articles/ArticleListItemViewModel.cs
Articles/ArticleDetailsViewModel.cs
Community/CommunityIndexViewModel.cs
Community/CommunityGroupViewModel.cs
Community/CreatePostViewModel.cs
Specialists/SpecialistListViewModel.cs
Specialists/SpecialistDetailsViewModel.cs
Specialists/ContactSpecialistViewModel.cs
Assistant/AssistantChatViewModel.cs
Assistant/SendAssistantMessageViewModel.cs
Activities/ActivityListViewModel.cs
```

View model rules:

- Use a view model for every form.
- Put validation attributes on form view models.
- Do not send database entities directly to complex views.
- Do not expose private user information in public view models.

### Controllers
Create controllers in `Controllers/`.

Recommended controllers:

```text
AccountController.cs
HomeController.cs
ProfileController.cs
HabitsController.cs
ArticlesController.cs
CommunityController.cs
SpecialistsController.cs
AssistantController.cs
ActivitiesController.cs
AdminController.cs
```

Controller responsibilities:

- Receive HTTP requests.
- Validate view models.
- Call services.
- Return views, redirects, or JSON.
- Stay small and readable.

Controllers should not:

- Store hardcoded lists of articles, specialists, groups, or posts.
- Calculate all scoring logic directly.
- Build large view models manually when a service can do it.
- Contain AI API logic directly.

### Views
Create Razor views in `Views/{ControllerName}/`.

Recommended views:

```text
Views/Home/Index.cshtml
Views/Home/Privacy.cshtml
Views/Account/Login.cshtml
Views/Account/Register.cshtml
Views/Profile/Index.cshtml
Views/Habits/Index.cshtml
Views/Articles/Index.cshtml
Views/Articles/Details.cshtml
Views/Community/Index.cshtml
Views/Community/Group.cshtml
Views/Specialists/Index.cshtml
Views/Specialists/Details.cshtml
Views/Specialists/Contact.cshtml
Views/Assistant/Index.cshtml
Views/Activities/Index.cshtml
Views/Activities/Breathing.cshtml
Views/Shared/_Layout.cshtml
Views/Shared/_ValidationScriptsPartial.cshtml
Views/Shared/Error.cshtml
```

View rules:

- Use strongly typed view models.
- Use Tag Helpers for forms and links.
- Use partial views for repeated cards or rows.
- Keep health content short and readable.
- Show validation messages beside form inputs.
- Use clear labels for accessibility.
- Keep the side menu available on desktop.
- Use a real mobile offcanvas menu on small screens.

### Services
Add a service layer. This makes the app easier to test and avoids large controllers.

Create:

```text
Services/Interfaces/IProfileService.cs
Services/Interfaces/IHabitService.cs
Services/Interfaces/IArticleService.cs
Services/Interfaces/ICommunityService.cs
Services/Interfaces/ISpecialistService.cs
Services/Interfaces/IAssistantService.cs
Services/Interfaces/IScoringService.cs
Services/Interfaces/IRecommendationService.cs

Services/Implementations/ProfileService.cs
Services/Implementations/HabitService.cs
Services/Implementations/ArticleService.cs
Services/Implementations/CommunityService.cs
Services/Implementations/SpecialistService.cs
Services/Implementations/AssistantService.cs
Services/Implementations/ScoringService.cs
Services/Implementations/RecommendationService.cs
```

Service responsibilities:

- `IProfileService`: build profile dashboard data.
- `IHabitService`: create habits, update logs, read daily and weekly progress.
- `IScoringService`: calculate scores, streaks, and monthly totals.
- `IRecommendationService`: create safe suggestions from habit and stat data.
- `IArticleService`: search, filter, and load articles.
- `ICommunityService`: join groups, create posts, moderate posts.
- `ISpecialistService`: search specialists and save contact requests.
- `IAssistantService`: save messages, return safe mock replies first, call a real LLM later.

Register services in `Program.cs`:

```csharp
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ISpecialistService, SpecialistService>();
builder.Services.AddScoped<IAssistantService, AssistantService>();
builder.Services.AddScoped<IScoringService, ScoringService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
```

## Database Setup

### Step 1: Add Connection String
In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WellYouthDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Use user secrets or environment variables for private production values later.

### Step 2: Create DbContext
Create `Data/ApplicationDbContext.cs`.

It should inherit from `IdentityDbContext<ApplicationUser>`.

Add DbSets:

```csharp
public DbSet<HealthHabit> HealthHabits => Set<HealthHabit>();
public DbSet<HealthHabitLog> HealthHabitLogs => Set<HealthHabitLog>();
public DbSet<HealthStat> HealthStats => Set<HealthStat>();
public DbSet<HealthScoreEntry> HealthScoreEntries => Set<HealthScoreEntry>();
public DbSet<Article> Articles => Set<Article>();
public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();
public DbSet<Specialist> Specialists => Set<Specialist>();
public DbSet<SpecialistContactRequest> SpecialistContactRequests => Set<SpecialistContactRequest>();
public DbSet<CommunityGroup> CommunityGroups => Set<CommunityGroup>();
public DbSet<CommunityGroupMember> CommunityGroupMembers => Set<CommunityGroupMember>();
public DbSet<CommunityPost> CommunityPosts => Set<CommunityPost>();
public DbSet<AssistantConversation> AssistantConversations => Set<AssistantConversation>();
public DbSet<AssistantMessage> AssistantMessages => Set<AssistantMessage>();
public DbSet<WellnessActivity> WellnessActivities => Set<WellnessActivity>();
public DbSet<UserActivityLog> UserActivityLogs => Set<UserActivityLog>();
```

### Step 3: Configure Program.cs
In `Program.cs`, configure:

- `ApplicationDbContext`.
- Identity.
- MVC controllers with views.
- Authentication.
- Authorization.
- Static files.
- Default route.

Recommended flow:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
```

For a school demo, confirmed account can stay false. For production, require confirmed email.

### Step 4: Define Relationships
Use real foreign keys.

Recommended relationships:

- `ApplicationUser` has many `HealthHabits`.
- `HealthHabit` has many `HealthHabitLogs`.
- `ApplicationUser` has many `HealthStats`.
- `ApplicationUser` has many `HealthScoreEntries`.
- `Article` belongs to `ArticleCategory`.
- `CommunityGroup` has many `CommunityPosts`.
- `CommunityGroup` has many `CommunityGroupMembers`.
- `ApplicationUser` has many `CommunityPosts`.
- `Specialist` has many `SpecialistContactRequests`.
- `ApplicationUser` has many `SpecialistContactRequests`.
- `ApplicationUser` has many `AssistantConversations`.
- `AssistantConversation` has many `AssistantMessages`.
- `WellnessActivity` has many `UserActivityLogs`.

Avoid text-only relationships such as storing only a group name on a community post.

### Step 5: Add Constraints
Use data annotations and Fluent API.

Recommended constraints:

- Habit name required, max length 80.
- One habit log per habit per date.
- Article title required, max length 160.
- Article summary required, max length 300.
- Article body required.
- Article slug unique.
- Community group name unique.
- Community post content required, max length 1000.
- Specialist name required, max length 120.
- Specialist specialty indexed.
- Contact request message max length 1000.

### Step 6: Add Audit Fields
For important entities, add:

- `CreatedAt`.
- `UpdatedAt`.
- `CreatedByUserId` where useful.
- `IsDeleted` or `ArchivedAt` when soft delete is needed.

Use these especially for:

- Habits.
- Articles.
- Community posts.
- Specialist contact requests.
- Assistant messages.

### Step 7: Seed Starter Data
Add seed data through `OnModelCreating` or a dedicated database seeding service.

Seed:

- Roles: `Admin`, `Moderator`, `User`.
- Article categories: Mental Health, Physical Health, Nutrition, Sleep, Stress, Exercise.
- Community groups: Healthy Eaters, Daily Walkers, Mindfulness, Sleep Support.
- Example specialists.
- Wellness activities: breathing timer, hydration challenge, mood check-in.

Do not store real private credentials directly in source code.

### Step 8: Create Migration
After models and DbContext are ready:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Rules:

- Review migration files before applying.
- Keep migrations small and named clearly.
- If local development data becomes messy early, it is acceptable to drop and recreate the local database.
- Once real data exists, use proper migrations instead of deleting the database.

## Suggested Database Tables

### Identity
Identity creates its own tables.

Extend `ApplicationUser` with:

- `FullName`.
- `AgeGroup` or `DateOfBirth`, only if needed.
- `HealthScore`.
- `CreatedAt`.

Avoid collecting sensitive health data unless the feature truly needs it.

### Health Tracking
`HealthHabits`

- `Id`.
- `UserId`.
- `Name`.
- `Description`.
- `Category`.
- `TargetFrequency`.
- `IsActive`.
- `CreatedAt`.

`HealthHabitLogs`

- `Id`.
- `HealthHabitId`.
- `UserId`.
- `LogDate`.
- `IsCompleted`.
- `Notes`.
- `CreatedAt`.

`HealthStats`

- `Id`.
- `UserId`.
- `StatType`.
- `Value`.
- `Unit`.
- `RecordedAt`.

`HealthScoreEntries`

- `Id`.
- `UserId`.
- `Score`.
- `Reason`.
- `ScoreDate`.

### Articles
`ArticleCategories`

- `Id`.
- `Name`.
- `Slug`.

`Articles`

- `Id`.
- `Title`.
- `Slug`.
- `Summary`.
- `Body`.
- `CategoryId`.
- `AuthorName`.
- `ReviewedBy`.
- `PublishedDate`.
- `Status`.
- `CreatedAt`.
- `UpdatedAt`.

### Community
`CommunityGroups`

- `Id`.
- `Name`.
- `Description`.
- `Guidelines`.
- `CreatedAt`.

`CommunityGroupMembers`

- `Id`.
- `GroupId`.
- `UserId`.
- `JoinedAt`.

`CommunityPosts`

- `Id`.
- `GroupId`.
- `UserId`.
- `Content`.
- `PostTime`.
- `Status`.
- `ModerationReason`.

Post statuses:

- Pending.
- Approved.
- Hidden.
- Flagged.

### Specialists
`Specialists`

- `Id`.
- `Name`.
- `Specialty`.
- `Description`.
- `ContactMethod`.
- `Location`.
- `IsVerified`.
- `CreatedAt`.

`SpecialistContactRequests`

- `Id`.
- `SpecialistId`.
- `UserId`.
- `Message`.
- `CreatedAt`.
- `Status`.

### Assistant
`AssistantConversations`

- `Id`.
- `UserId`.
- `Title`.
- `CreatedAt`.

`AssistantMessages`

- `Id`.
- `ConversationId`.
- `Sender`.
- `Content`.
- `SentAt`.
- `SafetyCategory`.

### Activities
`WellnessActivities`

- `Id`.
- `Title`.
- `Description`.
- `ActivityType`.
- `PointsValue`.
- `IsActive`.

`UserActivityLogs`

- `Id`.
- `UserId`.
- `ActivityId`.
- `CompletedAt`.
- `PointsEarned`.

## Build Phases

### Phase 1: Foundation
1. Create the new MVC project.
2. Add EF Core and Identity packages.
3. Create the folder structure.
4. Configure database connection.
5. Create `ApplicationDbContext`.
6. Configure Identity.
7. Configure dependency injection for services.
8. Build and run the empty app.

### Phase 2: Authentication
1. Create `ApplicationUser`.
2. Create login and register view models.
3. Build `AccountController`.
4. Build login and register views.
5. Add logout form.
6. Add validation.
7. Protect personal pages with `[Authorize]`.

### Phase 3: Layout And Navigation
1. Build `_Layout.cshtml`.
2. Add desktop sidebar navigation.
3. Add mobile offcanvas navigation.
4. Add floating AI assistant button.
5. Add active page styling.
6. Add accessible labels and focus states.
7. Test layout on desktop and mobile sizes.

### Phase 4: Home Dashboard
1. Build the first screen as useful app content, not only marketing text.
2. Show quick links to Profile, Community, Articles, Specialists, Activities, and AI Assistant.
3. Keep text short and clear.
4. Add a safety note that the platform supports health habits but does not replace professional care.

### Phase 5: Health Tracking And Profile
1. Create health tracking entities.
2. Create migrations.
3. Build `IHabitService`.
4. Build `IScoringService`.
5. Build profile dashboard view model.
6. Allow users to create habits.
7. Allow users to mark daily habits complete.
8. Save habit logs.
9. Show weekly and monthly progress.
10. Calculate health score from real activity.

### Phase 6: Articles
1. Create article and category entities.
2. Seed categories and sample reviewed articles.
3. Build `IArticleService`.
4. Build article list page.
5. Add search and category filtering.
6. Build article details page with full content.
7. Include reviewed-by or source information when available.

### Phase 7: Specialists
1. Create specialist and contact request entities.
2. Seed sample specialists.
3. Build `ISpecialistService`.
4. Build specialist list and details pages.
5. Add specialty and location filters.
6. Save contact requests from logged-in users.
7. Validate contact request messages.

### Phase 8: Community
1. Create group, member, and post entities.
2. Seed starter community groups.
3. Build `ICommunityService`.
4. Build community index page.
5. Build group details page.
6. Allow users to join groups.
7. Allow users to create posts.
8. Save new posts as `Pending` first.
9. Show only approved posts publicly.
10. Add safety guidelines and reporting/moderation notes.

### Phase 9: Activities And Monthly Competition
1. Create wellness activity and activity log entities.
2. Seed starter activities.
3. Build an activity list page.
4. Build at least one simple interactive activity, such as a breathing timer.
5. Save activity completion.
6. Award points through `IScoringService`.
7. Show monthly score on profile.
8. Add a leaderboard only if privacy is handled carefully.

### Phase 10: AI Assistant
1. Create assistant conversation and message entities.
2. Build `IAssistantService`.
3. First implementation should return safe mock responses.
4. Persist chat messages.
5. Add disclaimer that the assistant is not a doctor.
6. Add safety rules for urgent or risky topics.
7. Later connect the service to an LLM API.
8. Store API keys in user secrets or environment variables.
9. Recommend relevant articles and specialists from the database.

### Phase 11: Admin And Moderation
1. Add roles: Admin, Moderator, User.
2. Create basic admin pages only if time allows.
3. Let moderators approve, hide, or flag community posts.
4. Let admins manage articles and specialists.
5. Keep admin pages protected with role authorization.

### Phase 12: Testing And Polish
1. Run `dotnet build`.
2. Test registration, login, logout.
3. Test protected pages.
4. Test habit creation and completion.
5. Test score calculation.
6. Test article search and article details.
7. Test specialist filtering and contact requests.
8. Test community group join and post creation.
9. Test mobile navigation.
10. Check forms for labels, validation messages, keyboard navigation, and readable contrast.

## Security And Safety Rules
Because this is a youth health platform:

- Do not present AI responses as diagnosis.
- Do not tell users to ignore doctors or professional care.
- Add disclaimers on assistant, articles, and community pages.
- Add urgent-help guidance for crisis or self-harm language if mental health chat is supported.
- Require login for profile, habits, posting, contact requests, and saved assistant history.
- Use `[ValidateAntiForgeryToken]` on form posts.
- Validate all user input.
- Avoid storing unnecessary sensitive health details.
- Do not display user email publicly.
- Use moderation status before showing community posts publicly.
- Add lockout settings for repeated failed logins.

## UI And Design Rules
Design should be:

- Clean.
- Modern.
- Trustworthy.
- Friendly without looking childish.
- Easy to navigate.
- Usable on mobile.

Important UI rules:

- Keep the side menu as the main navigation on desktop.
- Use mobile offcanvas navigation on small screens.
- Keep the AI assistant reachable from both the side menu and a floating button.
- Make the home page useful immediately.
- Keep articles short and easy to scan.
- Add empty states like "No habits yet" or "No posts yet".
- Add loading states for assistant messages.
- Make buttons easy to tap.
- Use readable color contrast.
- Avoid broken encoding by saving files as UTF-8.

## Implementation Checklist

- [ ] Create new MVC project.
- [ ] Add EF Core and Identity packages.
- [ ] Create clean folders.
- [ ] Create entity models.
- [ ] Create view models.
- [ ] Create `ApplicationDbContext`.
- [ ] Configure database connection.
- [ ] Configure Identity.
- [ ] Register services.
- [ ] Add first migration.
- [ ] Update database.
- [ ] Seed roles and starter data.
- [ ] Build account pages.
- [ ] Build shared layout and navigation.
- [ ] Build home dashboard.
- [ ] Build profile dashboard.
- [ ] Build habit tracking.
- [ ] Build scoring service.
- [ ] Build articles.
- [ ] Build specialists.
- [ ] Build community.
- [ ] Build activities.
- [ ] Build assistant service and chat UI.
- [ ] Add moderation/admin basics if time allows.
- [ ] Run build.
- [ ] Run manual tests.
- [ ] Fix accessibility and mobile issues.

## Definition Of Done
The project is ready when:

- The app builds without errors.
- The app runs locally.
- Main pages work on desktop and mobile.
- Users can register, log in, and log out.
- Personal pages require login.
- Habits can be created and completed.
- Habit progress is saved in the database.
- Health score changes from real tracked activity.
- Articles load from the database.
- Article details have real content.
- Specialists load from the database.
- Specialist contact requests are saved.
- Community groups and posts are saved.
- Community posts use moderation status.
- The assistant uses a service layer.
- Assistant chat history can be saved.
- The assistant clearly says it is not medical care.
- The side menu and floating assistant button work.
- Basic testing or manual verification is documented.
