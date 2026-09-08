// ==========================================
// ESTADO GLOBAL Y VARIABLES
// ==========================================
let litaContactos = [];

// ==========================================
// ELEMENTOS DEL DOM
// ==========================================
// Seccion [Opciones]
const btnNew = document.querySelector("#btnNew"); // Boton para agregar persona
const inputFilter = document.querySelector("#inputFilter"); // Input para filtrar por ID

// Seccion [Tabla]
const tabla = document.querySelector("#tabla"); // Cuerpo de la tabla de personas

// Seccion [Modal]
const btnSave = document.querySelector("#btnSave"); // Boton para guardar cambios en el modal

const modal = {
  instance: null,
  id: document.querySelector("#personModal"), // Modal principal
  titulo: document.querySelector("#modalTitle"), // Titulo del modal
  body: document.querySelector("#modalBody"), // Cuerpo del modal
  footer: document.querySelector("#modalFooter"), // Pie del modal
  clear: () => {
    modal.titulo.textContent = "";
    modal.body.innerHTML = "";
    // Optimizacion: Correccion de error tipografico en la etiqueta button
    modal.footer.innerHTML = `<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>`;
  }
};


// ==========================================
// EVENTOS
// ==========================================
window.addEventListener('DOMContentLoaded', async () => {
  modal.instance = new bootstrap.Modal(modal.id);
  litaContactos = await getAll();
});

btnNew.addEventListener("click", () => {
  modal.clear();
  modal.titulo.textContent = "Agregar Persona";

  // Crear formulario
  const form = document.createElement("form");
  form.setAttribute("autocomplete", "on");

  const divName = crearInput({ id: "inputNombre", label: "Nombre" });
  const divTelefono = crearInput({ id: "inputTelefono", label: "Telefono", type: "number" });

  // Optimizacion: uso de append para multiples nodos
  form.append(divName, divTelefono);
  modal.body.appendChild(form);

  // Footer
  const btnSubmit = document.createElement("button");
  btnSubmit.setAttribute("type", "submit");
  btnSubmit.classList.add("btn", "btn-primary", "nuevoContacto");
  btnSubmit.textContent = "Guardar";

  modal.footer.appendChild(btnSubmit);
  modal.instance.show();
});

modal.footer.addEventListener("click", async (e) => {
  if (e.target.classList.contains("nuevoContacto")) {
    e.preventDefault();
    const data = {
      nombre: document.getElementById("inputNombre").value,
      telefono: document.getElementById("inputTelefono").value
    };
  }
});


// ==========================================
// FUNCIONES RECURSIVAS Y GENERALES
// ==========================================

// Cargar tabla
async function cargar() {
  litaContactos = await getAll();
  tabla.innerHTML = "";

  // Optimizacion: Uso de DocumentFragment para evitar multiples reflows del DOM
  const fragment = document.createDocumentFragment();

  litaContactos.forEach(contacto => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
  <td>${contacto.id}</td>
  <td>${contacto.nombre}</td>
  <td>${contacto.telefono}</td>
  <td><button class="btn btn-primary" data-edit="${contacto.id}">Editar</button></td>
  <td><button class="btn btn-danger" data-delete="${contacto.id}">Eliminar</button></td>
  `;
    fragment.appendChild(tr);
  });

  tabla.appendChild(fragment);
}

function crearInput({ id, label, type = "text" }) {
  const div = document.createElement("div");
  div.classList.add("mb-3");

  const labelElement = document.createElement("label");
  labelElement.setAttribute("for", id);
  labelElement.classList.add("form-label");
  labelElement.textContent = label;

  const input = document.createElement("input");
  input.type = type;
  input.classList.add("form-control");
  input.id = id;

  div.append(labelElement, input);
  return div;
}
