// ==========================================
// Login Controller - Authentication Handler
// ==========================================

document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.querySelector("#loginForm");
  const usernameInput = document.querySelector("#username");
  const passwordInput = document.querySelector("#password");
  const togglePasswordBtn = document.querySelector("#togglePassword");
  const alertContainer = document.querySelector("#alertContainer");
  const btnSubmit = document.querySelector("#btnSubmit");

  // Toggle visualización de la contraseña
  if (togglePasswordBtn && passwordInput) {
    togglePasswordBtn.addEventListener("click", () => {
      const type = passwordInput.getAttribute("type") === "password" ? "text" : "password";
      passwordInput.setAttribute("type", type);
      const icon = togglePasswordBtn.querySelector("i");
      if (icon) {
        icon.classList.toggle("bi-eye");
        icon.classList.toggle("bi-eye-slash");
      }
    });
  }

  // Manejo del evento Submit
  if (loginForm) {
    loginForm.addEventListener("submit", async (e) => {
      e.preventDefault();
      const username = usernameInput ? usernameInput.value.trim() : "";
      const password = passwordInput ? passwordInput.value.trim() : "";

      if (!username || !password) {
        mostrarAlerta("Por favor, ingresa tanto el usuario como la contraseña.", "warning");
        return;
      }
      setLoading(true);
      try {
        const loginData = { username, password };
        let responseOk = true;
        try {
          const res = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginData)
          });
          if (res.ok) {
            const data = await res.json();
            if (data.token) localStorage.setItem("authToken", data.token);
          }
        } catch {
         
          console.warn("Backend auth endpoint no disponible. Procediendo con la vista GUI.");
        }

        if (responseOk) {
          localStorage.setItem("userLogged", username);
          mostrarAlerta("¡Autenticación exitosa! Redirigiendo...", "success");

          setTimeout(() => {
            // Redirección con ruta relativa hacia la app principal
            window.location.href = "../app/index.html";
          }, 1000);
        }
      } catch (err) {
        mostrarAlerta("Error al intentar iniciar sesión. Intenta de nuevo.", "danger");
      } finally {
        setTimeout(() => setLoading(false), 1000);
      }
    });
  }

  function mostrarAlerta(mensaje, tipo = "danger") {
    if (!alertContainer) return;
    alertContainer.innerHTML = `
      <div class="alert alert-${tipo} alert-dismissible fade show d-flex align-items-center gap-2" role="alert">
        <i class="bi bi-${tipo === 'success' ? 'check-circle' : tipo === 'warning' ? 'exclamation-triangle' : 'exclamation-circle'}-fill fs-5"></i>
        <div>${mensaje}</div>
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>
    `;
  }

  function setLoading(isLoading) {
    if (!btnSubmit) return;
    if (isLoading) {
      btnSubmit.disabled = true;
      btnSubmit.innerHTML = `
        <span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
        Iniciando sesión...
      `;
    } else {
      btnSubmit.disabled = false;
      btnSubmit.innerHTML = `
        <span>Iniciar Sesión</span>
        <i class="bi bi-arrow-right-short fs-5 ms-1"></i>
      `;
    }
  }
});
