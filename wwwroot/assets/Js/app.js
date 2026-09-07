// Elementos de la seccion [opciones]
const btnNew = document.querySelector("#btnNew"); // Boton para agregar persona
const inputFilter = document.querySelector("#inputFilter"); // Input para filtrar por ID
// Elementos de la seccion [tabla]
const tabla = document.querySelector("#tabla"); // Cuerpo de la tabla de personas
// Elementos de la seccion [modal]
const modal = {
  instance: null,
  personModal: document.querySelector("#personModal"), // Modal principal
  modalTitle: document.querySelector("#modalTitle"), // Titulo del modal
  modalBody: document.querySelector("#modalBody"), // Cuerpo del modal
  modalFooter: document.querySelector("#modalFooter"), // Pie del modal
}


//eventos 
const btnSave = document.querySelector("#btnSave"); // Boton para guardar cambios en el modal
window.addEventListener('DOMContentLoaded', () => {
  modal.instance = new bootstrap.Modal(document.querySelector("#personModal"))
})

btnNew.addEventListener("click", () => {
  modal.modalTitle.textContent = "Agregar Persona"
  


  modal.instance.show()

})
