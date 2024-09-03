-- Desactivar temporalmente la verificación de claves foráneas
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

-- Insertar categorías
INSERT INTO Categorias (Nombre, Descripcion) VALUES 
('VIP', 'Invitados de alta prioridad con acceso completo a áreas exclusivas y servicios premium.'),
('Alta', 'Invitados importantes con acceso a la mayoría de áreas y servicios.'),
('Media', 'Invitados con acceso a áreas estándar y servicios básicos.'),
('Baja', 'Invitados con menor prioridad, con acceso a áreas comunes y servicios básicos.'),
('Sin Clasificar', 'Invitados sin categoría definida, incluyendo nuevos contactos.');

-- Insertar algunos invitados de ejemplo
INSERT INTO Invitados (Nombre, Profesion, Entretenimiento, Deporte, Alimentacion, CategoriaId) VALUES 
('Juan Flores', 'Ingeniero', 'Cine', 'Fútbol', 'Vegetariano', 1),
('María Hernández', 'Doctora', 'Teatro', 'Natación', 'Vegano', 1),
('Ana Víquez', 'Arquitecta', 'Museos', 'Ciclismo', 'Vegetariano', 2),
('Krissia Fernández', 'Contadora', 'Lectura', 'Baloncesto', 'Vegetariano', 3),
('Fabio Herrera', 'Electricista', 'Pesca', 'Caminata', 'Omnívoro', 4),
('Fernando Hernández', 'Carpintero', 'Videojuegos', 'Baloncesto', 'Omnívoro', 5);

-- Insertar algunos eventos de ejemplo
INSERT INTO Eventos (Nombre, Fecha, HoraInicio, HoraFin, CupoLimite, Tema, Lugar, Medio, TipoEvento) VALUES
('Capacitación Anual', '2024-09-15', '09:00', '17:00', 100, 'Nuevas tecnologías', 'Salón Principal', 'Presencial', 'Capacitación Técnica'),
('Charla Motivacional', '2024-10-01', '14:00', '16:00', 50, 'Liderazgo efectivo', 'Auditorio', 'Presencial', 'Charla RRHH'),
('Reunión Trimestral', '2024-11-30', '10:00', '12:00', 200, 'Resultados Q4', 'Virtual', 'Virtual', 'Reunión General');

-- Insertar algunas suscripciones de ejemplo
INSERT INTO Suscripciones (InvitadoId, EventoId) VALUES
(1, 1), (1, 2), (2, 1), (3, 2), (4, 3), (5, 3);

-- Insertar usuarios de ejemplo
INSERT INTO Usuarios (Username, Password, Rol) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin'),
('usuario', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Usuario');

-- Reactivar la verificación de claves foráneas y comprobar la integridad
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL"