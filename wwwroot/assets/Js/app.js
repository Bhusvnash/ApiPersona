// ==========================================
// ESTADO GLOBAL Y VARIABLES
// ==========================================
let listaPersonas = [];

// ELEMENTOS DEL DOM
const btnNew = document.querySelector("#btnNew");
const inputFilter = document.querySelector("#inputFilter");
const tabla = document.querySelector("#tabla");
const modal = {
  instance: null,
  element: document.querySelector('#personModal'),
  titulo: document.querySelector('#modalTitle'),
  body: document.querySelector("#modalBody"),
  footer: document.querySelector("#modalFooter"),
  //metodos
  clear: () => {
    modal.titulo.textContent = ``
    modal.body.innerHTML = ``
    modal.footer.innerHTML = `<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>`
  },
  make: () => {
    if (!modal.instance) {
      modal.instance = new bootstrap.Modal(modal.element)
    }
  },
  show: () => {
    modal.instance.show()
  },
  hide: () => {
    modal.instance.hide()
  }
}
function modalNuevaPersona() {

  modal.clear();
  modal.titulo.textContent = "Agregar Persona";
  //make form using doom
  const form = document.createElement('form');
  //crearInput({ id, label, type = "text" }
  const divNombre = crearInput({ id: "inputNombre", label: "Nombre" })
  const divTelefono = crearInput({ id: "inputTelefono", label: "Telefono", type: 'tel' })
  form.appendChild(divNombre)
  form.appendChild(divTelefono)
  //buttons 
  const btnSave = crearButton({ id: '', text: 'Guardar', accion: 'agregarContacto' })
  modal.body.appendChild(form);
  modal.footer.appendChild(btnSave);
  modal.show();
}

// ==========================================
// FUNCIONES RECURSIVAS Y GENERALES
// ==========================================
function crearFila(personas = listaPersonas) {
  if (personas.length === 0) {
    console.log(personas); return
  }
  personas.forEach(person => {
    const tr = document.createElement('tr')
    //
    const tdId = document.createElement("td");
    const tdNombre = document.createElement("td");
    const tdTelefono = document.createElement("td");
    const tdAcciones = document.createElement("td");
    //editar,eliminar
    //txt
    tdId.textContent = person.id
    tdNombre.textContent = person.nombre
    tdTelefono.textContent = person.telefono
    //
    const btnEliminar = crearButton({
      id: "btnEliminar",
      text: "Eliminar",
      accion: "eliminar",
      color: "btn-danger"
    });
    const btnEditar = crearButton({
      id: "btnEditar",
      text: "Editar",
      accion: "editar",
      color: "btn-success"
    });
    btnEliminar.setAttribute('data-id', person.id);
    btnEditar.setAttribute('data-id', person.id);
    //
    tdAcciones.append(btnEditar, btnEliminar);
    tr.appendChild(tdId);
    tr.appendChild(tdNombre);
    tr.appendChild(tdTelefono);
    tr.appendChild(tdAcciones);
    tabla.appendChild(tr);
  })
}
function crearButton({ id, text, accion, color }) {
  color = color ?? "btn-primary";
  const button = document.createElement("button");
  button.classList.add(color, accion, 'btn', 'm-1');
  button.textContent = text;
  button.id = id
  //text : crearButton({id:btn,text:"texto",accion:"test",color:"btn-warnig"});
  return button
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

