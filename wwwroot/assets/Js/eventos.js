// ==========================================
// EVENTOS - Delegación y Manejo de Eventos
// Responsabilidad: Registrar event listeners, delegar acciones y llamar funciones de app.js
// ==========================================

document.addEventListener("DOMContentLoaded", async () => {
  modal.init();
  await cargarPersonas();
});

// Evento: Click en el botón "+ Agregar"
if (btnNew) {
  btnNew.addEventListener("click", () => {
    modalNuevaPersona();
  });
}

// Evento: Filtro por ID o Nombre en tiempo real
if (inputFilter) {
  inputFilter.addEventListener("input", (e) => {
    filtrarPersonas(e.target.value);
  });
}

// Delegación de Eventos: Clicks en la Tabla (Editar / Eliminar)
if (tabla) {
  tabla.addEventListener("click", (e) => {
    const target = e.target.closest("button[data-accion]");
    if (!target) return;

    const accion = target.getAttribute("data-accion");
    const id = target.getAttribute("data-id");

    if (accion === "editar") {
      modalEditarPersona(id);
    } else if (accion === "eliminar") {
      procesarEliminarPersona(id);
    }
  });
}

// Delegación de Eventos: Clicks en el Footer del Modal (Guardar / Actualizar)
if (modal.footer) {
  modal.footer.addEventListener("click", (e) => {
    const target = e.target.closest("button[data-accion]");
    if (!target) return;

    const accion = target.getAttribute("data-accion");
    const id = target.getAttribute("data-id");

    if (accion === "guardarContacto") {
      procesarGuardarPersona();
    } else if (accion === "actualizarContacto") {
      procesarActualizarPersona(id);
    }
  });
}