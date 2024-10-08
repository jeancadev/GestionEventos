-- Desactivar temporalmente la verificaci�n de claves for�neas
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL"

-- Limpiar datos existentes
DELETE FROM Suscripciones;
DELETE FROM Invitados;
DELETE FROM Eventos;
DELETE FROM Categorias;
DELETE FROM Usuarios;

-- Reiniciar los contadores de identidad
DBCC CHECKIDENT ('Suscripciones', RESEED, 0);
DBCC CHECKIDENT ('Invitados', RESEED, 0);
DBCC CHECKIDENT ('Eventos', RESEED, 0);
DBCC CHECKIDENT ('Categorias', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);

-- Insertar categor�as
INSERT INTO Categorias (Nombre, Descripcion) VALUES 
('VIP', 'Invitados de alta prioridad con acceso completo a �reas exclusivas y servicios premium.'),
('Alta', 'Invitados importantes con acceso a la mayor�a de �reas y servicios.'),
('Media', 'Invitados con acceso a �reas est�ndar y servicios b�sicos.'),
('Baja', 'Invitados con menor prioridad, con acceso a �reas comunes y servicios b�sicos.'),
('Sin Clasificar', 'Invitados sin categor�a definida, incluyendo nuevos contactos.');

-- Insertar algunos invitados de ejemplo
INSERT INTO Invitados (Nombre, Profesion, Entretenimiento, Deporte, Alimentacion, CategoriaId) VALUES 
('Juan Flores', 'Ingeniero', 'Cine', 'F�tbol', 'Vegetariano', 1),
('Mar�a Hern�ndez', 'Doctora', 'Teatro', 'Nataci�n', 'Vegano', 1),
('Ana V�quez', 'Arquitecta', 'Museos', 'Ciclismo', 'Vegetariano', 2),
('Krissia Fern�ndez', 'Contadora', 'Lectura', 'Baloncesto', 'Vegetariano', 3),
('Fabio Herrera', 'Electricista', 'Pesca', 'Caminata', 'Omn�voro', 4),
('Fernando Hern�ndez', 'Carpintero', 'Videojuegos', 'Baloncesto', 'Omn�voro', 5);

-- Insertar algunos eventos de ejemplo
INSERT INTO Eventos (Nombre, Fecha, HoraInicio, HoraFin, CupoLimite, Tema, Lugar, Medio, TipoEvento) VALUES
('Capacitaci�n Anual', '2024-09-15', '09:00', '17:00', 100, 'Nuevas tecnolog�as', 'Sal�n Principal', 'Presencial', 'Capacitaci�n T�cnica'),
('Charla Motivacional', '2024-10-01', '14:00', '16:00', 50, 'Liderazgo efectivo', 'Auditorio', 'Presencial', 'Charla RRHH'),
('Reuni�n Trimestral', '2024-11-30', '10:00', '12:00', 200, 'Resultados Q4', 'Virtual', 'Virtual', 'Reuni�n General');

-- Insertar algunas suscripciones de ejemplo
INSERT INTO Suscripciones (InvitadoId, EventoId) VALUES
(1, 1), (1, 2), (2, 1), (3, 2), (4, 3), (5, 3);

-- Insertar usuarios de ejemplo
INSERT INTO Usuarios (Username, Password, Rol) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin'),
('usuario', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Usuario');

-- Reactivar la verificaci�n de claves for�neas y comprobar la integridad
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL"