using WellYouth.Data;
using WellYouth.Models.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace WellYouth.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.ArticleCategories.Any())
            {
                var categories = new List<ArticleCategory>
                {
                    new ArticleCategory { Name = "Mental Well-being", Slug = "mental-wellbeing" },
                    new ArticleCategory { Name = "Physical Fitness", Slug = "physical-fitness" },
                    new ArticleCategory { Name = "Healthy Nutrition", Slug = "nutrition" },
                    new ArticleCategory { Name = "Sleep Hygiene", Slug = "sleep" }
                };
                context.ArticleCategories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Bulk import from Articles.json if it exists
            string articlesPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Articles.json");
            // If running in production/built, might need a different path, but for dev this works.
            // Let's also check current directory
            if (!File.Exists(articlesPath)) articlesPath = "Articles.json";

            if (File.Exists(articlesPath))
            {
                var json = await File.ReadAllTextAsync(articlesPath);
                var importData = JsonSerializer.Deserialize<List<ArticleImportModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (importData != null)
                {
                    var categories = await context.ArticleCategories.ToListAsync();
                    foreach (var item in importData)
                    {
                        if (!context.Articles.Any(a => a.Slug == item.Slug))
                        {
                            var category = categories.FirstOrDefault(c => c.Slug == item.CategorySlug);
                            if (category != null)
                            {
                                context.Articles.Add(new Article
                                {
                                    Title = item.Title,
                                    Slug = item.Slug,
                                    Summary = item.Summary,
                                    Body = item.Body,
                                    CategoryId = category.Id,
                                    AuthorName = item.AuthorName,
                                    Source = item.Source,
                                    SourceUrl = item.SourceUrl,
                                    PublishedDate = DateTime.UtcNow,
                                    Status = item.Status ?? "Published",
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                });
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Articles.Any())
            {
                var cat1 = context.ArticleCategories.First(c => c.Slug == "mental-wellbeing").Id;
                var cat2 = context.ArticleCategories.First(c => c.Slug == "physical-fitness").Id;

                var articles = new List<Article>
                {
                    new Article { 
                        Title = "Understanding Mindfulness for Teens", 
                        Slug = "mindfulness-for-teens",
                        Summary = "How mindfulness can help you navigate the stresses of school and life.",
                        Body = "Mindfulness is the practice of being present... (detailed content)",
                        CategoryId = cat1,
                        AuthorName = "Dr. Sarah Smith",
                        PublishedDate = DateTime.UtcNow,
                        Status = "Published",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Article { 
                        Title = "Quick 15-Minute Home Workouts", 
                        Slug = "home-workouts",
                        Summary = "Stay active even with a busy schedule with these simple exercises.",
                        Body = "You don't need a gym to stay fit... (detailed content)",
                        CategoryId = cat2,
                        AuthorName = "Alex Trainer",
                        PublishedDate = DateTime.UtcNow,
                        Status = "Published",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };
                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();
            }

            // Aggressive Specialist Update: Remove old mock data and add real specialists
            var oldMockSpecialists = context.Specialists.Where(s => s.Name == "Dr. John Doe" || s.Name == "Jane Nutri").ToList();
            if (oldMockSpecialists.Any())
            {
                // First, remove any contact requests pointing to these specialists to avoid FK constraints
                var specIds = oldMockSpecialists.Select(s => s.Id).ToList();
                var orphanedRequests = context.SpecialistContactRequests.Where(r => specIds.Contains(r.SpecialistId)).ToList();
                if (orphanedRequests.Any())
                {
                    context.SpecialistContactRequests.RemoveRange(orphanedRequests);
                    await context.SaveChangesAsync(); // Commit request deletion first
                }
                
                context.Specialists.RemoveRange(oldMockSpecialists);
                await context.SaveChangesAsync(); // Now safe to delete specialists
            }

            var realSpecialists = new List<Specialist>
            {
                // PSYCHIATRY
                new Specialist { 
                    Name = "Khaled Abdulrahman", 
                    Specialty = "Psychiatry", 
                    Description = "Senior Specialist – Psychiatry. Age Group: 12 years and older.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                },
                // PSYCHOLOGICAL COUNSELORS
                new Specialist { 
                    Name = "Mohammad Ayman Omar", 
                    Specialty = "Psychological Counselor", 
                    Description = "Master’s Degree in Psychology (Clinical Track). Serves: Males only.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Specialist { 
                    Name = "Rana Al-Hawari", 
                    Specialty = "Psychological Counselor", 
                    Description = "Master’s Degree in Clinical Psychology. Age Group: 16 years and older. Serves: Females only.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Specialist { 
                    Name = "Zainab Al-Yamani", 
                    Specialty = "Psychological Counselor", 
                    Description = "Bachelor’s Degree in Psychology. Age Group: 12 years and older. Serves: Females only.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                },
                // OCCUPATIONAL THERAPISTS
                new Specialist { 
                    Name = "Razan Al-Tarawneh", 
                    Specialty = "Occupational Therapist", 
                    Description = "Occupational Therapist. Age Group: Up to 13 years old.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Specialist { 
                    Name = "Wafaa Al-Akhras", 
                    Specialty = "Occupational Therapist", 
                    Description = "Occupational Therapist. Age Group: Up to 9 years old.",
                    ContactMethod = "info@wanafs.com",
                    Location = "Amman, Jordan",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var spec in realSpecialists)
            {
                if (!context.Specialists.Any(s => s.Name == spec.Name))
                {
                    context.Specialists.Add(spec);
                }
            }
            await context.SaveChangesAsync();

            var communityGroups = new List<CommunityGroup>
            {
                new CommunityGroup { 
                    Name = "Mindful Students", 
                    Description = "A safe space to discuss school stress and mindfulness techniques.",
                    Guidelines = "Be respectful, no bullying, keep it supportive.",
                    CreatedAt = DateTime.UtcNow
                },
                new CommunityGroup { 
                    Name = "Active Youth", 
                    Description = "Share your fitness journey and find workout buddies.",
                    Guidelines = "Encourage others, share tips, no spam.",
                    CreatedAt = DateTime.UtcNow
                },
                new CommunityGroup { 
                    Name = "Healthy Eaters", 
                    Description = "Exchange recipes, meal prep tips, and discuss balanced nutrition for a healthy lifestyle.",
                    Guidelines = "Share accurate info, be supportive, no diet-shaming.",
                    CreatedAt = DateTime.UtcNow
                },
                new CommunityGroup { 
                    Name = "Sleep Better", 
                    Description = "A group dedicated to improving sleep habits, sharing wind-down routines, and discussing the importance of rest.",
                    Guidelines = "Keep discussions helpful and evidence-based where possible.",
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var group in communityGroups)
            {
                if (!context.CommunityGroups.Any(g => g.Name == group.Name))
                {
                    context.CommunityGroups.Add(group);
                }
            }
            await context.SaveChangesAsync();

            var activitySeeds = new List<WellnessActivity>
            {
                new WellnessActivity { 
                    Title = "Deep Breathing", 
                    Description = "A 60-second guided breathing exercise to reduce stress.",
                    ActivityType = "Breathing",
                    PointsValue = 10,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Mood Check-In", 
                    Description = "Take a moment to acknowledge how you're feeling right now.",
                    ActivityType = "MoodCheckIn",
                    PointsValue = 8,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Hydration Challenge", 
                    Description = "Drink a glass of water and stay hydrated for better focus.",
                    ActivityType = "Hydration",
                    PointsValue = 7,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Mindful Minute", 
                    Description = "Practice awareness and presence for 60 seconds.",
                    ActivityType = "Mindfulness",
                    PointsValue = 10,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Quick Stretch", 
                    Description = "Release physical tension with three simple stretches.",
                    ActivityType = "Stretch",
                    PointsValue = 12,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Gratitude Note", 
                    Description = "Reflect on one thing you're thankful for today.",
                    ActivityType = "Gratitude",
                    PointsValue = 8,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Sleep Wind-Down", 
                    Description = "Prepare your mind and body for a restful night.",
                    ActivityType = "SleepWindDown",
                    PointsValue = 10,
                    IsActive = true
                },
                new WellnessActivity { 
                    Title = "Positive Focus", 
                    Description = "Choose one positive action to focus on for the next hour.",
                    ActivityType = "PositiveFocus",
                    PointsValue = 8,
                    IsActive = true
                }
            };

            foreach (var seed in activitySeeds)
            {
                if (!context.WellnessActivities.Any(a => a.ActivityType == seed.ActivityType))
                {
                    context.WellnessActivities.Add(seed);
                }
            }
            await context.SaveChangesAsync();
            }
            }

            public class ArticleImportModel
            {
            public string Title { get; set; } = string.Empty;
            public string Slug { get; set; } = string.Empty;
            public string Summary { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
            public string CategorySlug { get; set; } = string.Empty;
            public string? AuthorName { get; set; }
            public string? Source { get; set; }
            public string? SourceUrl { get; set; }
            public string? Status { get; set; }
            }
            }