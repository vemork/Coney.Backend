1. en todas las creaciones retornar 201 - OK
2. ajustar el colletion al formato de los request los paths { { Url} } - OK
3. agregar comentario pa que sirve cada campo - OK
4. repositorios - User* - Country* - Riffle* - User x Riffle* - OK
5. modificar backend para comprar el puesto a jugar y que sea almacenado en user x rifa - OK
6. ajusatr campor siempre a 100 en rifas - OK (Eliminado)

# User
  - para crear enviar en el body 
      {
        "email": "doy@gmail.com",
        "firstName": "Doyler",
        "lastName": "vasquez",
        "password": "123456"
      }
  - para buscar indicar id por URL
  - para actualizar indicar id por URL y en el body 
      {
          "firstName": "Dev",
          "lastName": "Vas"
      }
  - para eliminar indicar id por URL 
# Country
  - para crear enviar en el body { "name": "Bolivia" } 
  - para buscar indicar id por URL
  - para actualizar indicar id por URL y en el body { "name": "Bolivia" }
  - para eliminar indicar id por URL 
# Riffle
  - para crear enviar en el body 
      {
          "Name": "Rifa4",
          "Description": "Rifa pro fondos coljuegos 2",
          "InitDate": "2024-09-26T14:30:00Z",
          "EndtDate": "2024-10-26T14:30:00Z"
      }
  - para buscar indicar id por URL
  - para actualizar indicar id por URL y en el body 
      {
          "Name": "Rifa 2 para gamers",
          "Description": "Rifa pro fondos mundial de e sports 2",
          "InitDate": "2024-09-26T14:30:00Z",
          "EndtDate": "2024-10-26T14:30:00Z"
      }
  - para eliminar indicar id por URL 
# Ticket
  - para crear enviar en el body 
      {
        "ticketNumber": "2",
        "riffleId": 1,
        "userId": 2
      }
  - para buscar indicar id por URL
  - para actualizar indicar id por URL y en el body 
      {
        "ticketNumber": "3",
        "riffleId": 1,
        "userId": 2
      }
  - para eliminar indicar id por URL 