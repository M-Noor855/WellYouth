document.addEventListener("DOMContentLoaded", () => {
    initThemeToggle();
    initNavigation();
    initDialogs();
    initBreathingTimer();
    initActivities();
    scrollChatToBottom();
});

function initActivities() {
    // Shared completion button enabler
    const enableComplete = (container) => {
        const btn = container.querySelector("[data-activity-complete]");
        if (btn) btn.disabled = false;
    };

    // Mood Check-In
    const moodRoot = document.querySelector("[data-mood-root]");
    if (moodRoot) {
        const options = moodRoot.querySelectorAll("[data-mood-option]");
        options.forEach(opt => {
            opt.addEventListener("click", () => {
                options.forEach(o => o.classList.remove("active"));
                opt.classList.add("active");
                enableComplete(moodRoot);
            });
        });
    }

    // Hydration
    const hydrationCheck = document.querySelector("[data-hydration-check]");
    if (hydrationCheck) {
        hydrationCheck.addEventListener("change", () => {
            if (hydrationCheck.checked) {
                enableComplete(hydrationCheck.closest(".surface-card"));
            } else {
                const btn = hydrationCheck.closest(".surface-card").querySelector("[data-activity-complete]");
                if (btn) btn.disabled = true;
            }
        });
    }

    // Mindfulness Timer
    const mindfulnessRoot = document.querySelector("[data-mindfulness-root]");
    if (mindfulnessRoot) {
        const startBtn = mindfulnessRoot.querySelector("[data-mindfulness-start]");
        const timerDisplay = mindfulnessRoot.querySelector("[data-mindfulness-timer]");
        
        if (startBtn && timerDisplay) {
            startBtn.addEventListener("click", () => {
                startBtn.disabled = true;
                startBtn.textContent = "Focusing...";
                let timeLeft = 60;
                
                const interval = setInterval(() => {
                    timeLeft--;
                    timerDisplay.textContent = timeLeft;
                    
                    if (timeLeft <= 0) {
                        clearInterval(interval);
                        timerDisplay.textContent = "0";
                        enableComplete(mindfulnessRoot);
                        startBtn.textContent = "Minute Complete";
                    }
                }, 1000);
            });
        }
    }

    // Stretch Checklist
    const stretchSteps = document.querySelectorAll("[data-stretch-step]");
    if (stretchSteps.length > 0) {
        stretchSteps.forEach(step => {
            step.addEventListener("change", () => {
                const allChecked = Array.from(stretchSteps).every(s => s.checked);
                if (allChecked) {
                    enableComplete(step.closest(".surface-card"));
                }
            });
        });
    }

    // Gratitude Note
    const gratitudeInput = document.querySelector("[data-gratitude-input]");
    if (gratitudeInput) {
        gratitudeInput.addEventListener("input", () => {
            if (gratitudeInput.value.trim().length > 0) {
                enableComplete(gratitudeInput.closest(".surface-card"));
            } else {
                const btn = gratitudeInput.closest(".surface-card").querySelector("[data-activity-complete]");
                if (btn) btn.disabled = true;
            }
        });
    }

    // Sleep Wind-Down Checklist
    const sleepSteps = document.querySelectorAll("[data-sleep-step]");
    if (sleepSteps.length > 0) {
        sleepSteps.forEach(step => {
            step.addEventListener("change", () => {
                const allChecked = Array.from(sleepSteps).every(s => s.checked);
                if (allChecked) {
                    enableComplete(step.closest(".surface-card"));
                }
            });
        });
    }

    // Positive Focus
    const focusOptions = document.querySelectorAll("[data-focus-option]");
    if (focusOptions.length > 0) {
        focusOptions.forEach(opt => {
            opt.addEventListener("click", () => {
                focusOptions.forEach(o => o.classList.remove("active"));
                opt.classList.add("active");
                enableComplete(opt.closest(".surface-card"));
            });
        });
    }
}


function initThemeToggle() {
    const toggle = document.getElementById("themeToggle");
    const root = document.documentElement;

    if (!toggle) {
        return;
    }

    const savedTheme = localStorage.getItem("theme");
    const initialTheme = savedTheme || "light";
    root.setAttribute("data-theme", initialTheme);
    updateThemeIcon(toggle, initialTheme);

    toggle.addEventListener("click", () => {
        const nextTheme = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
        root.setAttribute("data-theme", nextTheme);
        localStorage.setItem("theme", nextTheme);
        updateThemeIcon(toggle, nextTheme);
    });
}

function updateThemeIcon(toggle, theme) {
    const icon = toggle.querySelector("i");
    if (!icon) {
        return;
    }

    icon.className = theme === "dark" ? "fas fa-sun" : "fas fa-moon";
}

function initNavigation() {
    const toggle = document.querySelector("[data-nav-toggle]");
    const panel = document.getElementById("site-nav-panel");

    if (!toggle || !panel) {
        return;
    }

    toggle.addEventListener("click", () => {
        const isOpen = panel.classList.toggle("is-open");
        toggle.setAttribute("aria-expanded", String(isOpen));
    });

    panel.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", () => {
            panel.classList.remove("is-open");
            toggle.setAttribute("aria-expanded", "false");
        });
    });

    document.addEventListener("keydown", event => {
        if (event.key === "Escape") {
            panel.classList.remove("is-open");
            toggle.setAttribute("aria-expanded", "false");
        }
    });
}

function initDialogs() {
    const openButtons = document.querySelectorAll("[data-dialog-open]");
    const closeButtons = document.querySelectorAll("[data-dialog-close]");

    openButtons.forEach(button => {
        button.addEventListener("click", () => {
            const id = button.getAttribute("data-dialog-open");
            const dialog = document.getElementById(id);
            if (dialog) {
                dialog.classList.add("is-open");
                document.body.style.overflow = "hidden";
            }
        });
    });

    closeButtons.forEach(button => {
        button.addEventListener("click", () => closeDialog(button.closest(".dialog-backdrop")));
    });

    document.querySelectorAll(".dialog-backdrop").forEach(backdrop => {
        backdrop.addEventListener("click", event => {
            if (event.target === backdrop) {
                closeDialog(backdrop);
            }
        });
    });

    document.addEventListener("keydown", event => {
        if (event.key === "Escape") {
            document.querySelectorAll(".dialog-backdrop.is-open").forEach(closeDialog);
        }
    });
}

function closeDialog(backdrop) {
    if (!backdrop) {
        return;
    }

    backdrop.classList.remove("is-open");
    document.body.style.overflow = "";
}

function initBreathingTimer() {
    const root = document.querySelector("[data-breathing-root]");
    if (!root) {
        return;
    }

    let timeLeft = 60;
    let cycle = 0;
    const timer = root.querySelector("[data-breathing-timer]");
    const instruction = root.querySelector("[data-breathing-instruction]");
    const circle = root.querySelector("[data-breathing-circle]");
    const form = root.querySelector("form");

    const start = () => {
        const interval = window.setInterval(() => {
            timeLeft -= 1;

            if (timer) {
                timer.textContent = String(timeLeft);
            }

            if (timeLeft % 4 === 0 && circle && instruction) {
                const breathingIn = cycle % 2 === 0;
                circle.style.transform = breathingIn ? "scale(1.72)" : "scale(1)";
                circle.style.boxShadow = breathingIn
                    ? "0 0 0 30px rgba(21, 122, 110, 0.12)"
                    : "0 0 0 16px rgba(21, 122, 110, 0.08)";
                instruction.textContent = breathingIn ? "Breathe in slowly" : "Breathe out gently";
                cycle += 1;
            }

            if (timeLeft <= 0) {
                window.clearInterval(interval);
                if (instruction) {
                    instruction.textContent = "Session complete. Saving your progress...";
                }
                window.setTimeout(() => form?.submit(), 1200);
            }
        }, 1000);
    };

    window.setTimeout(start, 1200);
}

function scrollChatToBottom() {
    const chatWindow = document.getElementById("chat-messages");
    if (chatWindow) {
        chatWindow.scrollTop = chatWindow.scrollHeight;
    }
}
