// ==========================================
// DOOM (DOM) - Fábrica de Elementos Reutilizables (Atomic Design)
// Responsabilidad: Crear y estructurar componentes visuales de la UI
// ==========================================

// --- ÁTOMOS ---

/**
 * Átomo: Crea un Botón configurable
 */
function crearButton({ id = "", text = "", accion = "", color = "btn-primary", type = "button", dataId = "" }) {
  const button = document.createElement("button");
  button.type = type;
  button.className = `btn ${color} m-1`;
  if (id) button.id = id;
  if (accion) button.setAttribute("data-accion", accion);
  if (dataId !== "") button.setAttribute("data-id", dataId);
  button.textContent = text;
  return button;
}

/**
 * Átomo: Crea un Campo de Entrada (Input) con su Etiqueta (Label)
 */
function crearInput({ id, label, type = "text", value = "", placeholder = "" }) {
  const div = document.createElement("div");
  div.className = "mb-3";

  const labelElement = document.createElement("label");
  labelElement.setAttribute("for", id);
  labelElement.className = "form-label fw-semibold";
  labelElement.textContent = label;

  const input = document.createElement("input");
  input.type = type;
  input.className = "form-control";
  input.id = id;
  input.value = value;
  if (placeholder) input.placeholder = placeholder;

  div.append(labelElement, input);
  return div;
}

// --- MOLÉCULAS ---

/**
 * Molécula: Crea una Fila (tr) para la tabla de personas
 */
function crearFilaTabla(person) {
  const tr = document.createElement("tr");

  const tdId = document.createElement("td");
  tdId.className = "fw-bold text-secondary";
  tdId.textContent = `#${person.id}`;

  const tdNombre = document.createElement("td");
  tdNombre.textContent = person.nombre;

  const tdTelefono = document.createElement("td");
  tdTelefono.textContent = person.telefono;

  const tdAcciones = document.createElement("td");

  const btnEditar = crearButton({
    text: "Editar",
    accion: "editar",
    color: "btn-outline-warning btn-sm",
    dataId: person.id
  });

  const btnEliminar = crearButton({
    text: "Eliminar",
    accion: "eliminar",
    color: "btn-outline-danger btn-sm",
    dataId: person.id
  });

  tdAcciones.append(btnEditar, btnEliminar);
  tr.append(tdId, tdNombre, tdTelefono, tdAcciones);
  return tr;
}

/**
 * Molécula: Crea una fila vacía indicando que no hay registros
 */
function crearFilaVaciaTabla(mensaje = "No se encontraron datos") {
  const tr = document.createElement("tr");
  const td = document.createElement("td");
  td.colSpan = 4;
  td.className = "text-center py-4 text-muted fs-5";
  td.textContent = mensaje;
  tr.appendChild(td);
  return tr;
}

// --- ORGANISMOS ---

/**
 * Organismo: Formulario para Crear / Editar Persona
 */
function crearFormularioPersona(personaData = { id: "", nombre: "", telefono: "" }) {
  const form = document.createElement("form");
  form.id = "formPersona";

  if (personaData.id) {
    const inputIdHidden = document.createElement("input");
    inputIdHidden.type = "hidden";
    inputIdHidden.id = "inputId";
    inputIdHidden.value = personaData.id;
    form.appendChild(inputIdHidden);
  }

  const divNombre = crearInput({
    id: "inputNombre",
    label: "Nombre Completo",
    value: personaData.nombre || "",
    placeholder: "Ej. Juan Pérez"
  });

  const divTelefono = crearInput({
    id: "inputTelefono",
    label: "Teléfono de Contacto",
    type: "tel",
    value: personaData.telefono || "",
    placeholder: "Ej. 3001234567"
  });

  form.append(divNombre, divTelefono);
  return form;
}
