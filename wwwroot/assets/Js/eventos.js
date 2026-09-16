document.addEventListener("DOMContentLoaded", async (e) => {
  modal.make()
  listaPersonas = await getAll();
  //loadTable 
  crearFila(listaPersonas)
});

btnNew.addEventListener('click',(e)=>{
  console.log(e.target)
  modalNuevaPersona()
})


