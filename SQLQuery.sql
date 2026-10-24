use BibliotecaDb;
-- Tabla Estudiantes
CREATE TABLE Estudiantes (
    IdEstudiante INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Matricula NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL
);

-- Tabla Libros
CREATE TABLE Libros (
    IdLibro INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(150) NOT NULL,
    Autor NVARCHAR(100) NOT NULL,
    Stock INT NOT NULL DEFAULT 1
);

-- Tabla Prestamos
CREATE TABLE Prestamos (
    IdPrestamo INT IDENTITY(1,1) PRIMARY KEY,
    IdEstudiante INT NOT NULL,
    IdLibro INT NOT NULL,
    FechaPrestamo DATETIME NOT NULL DEFAULT GETDATE(),
    FechaDevolucionEsperada DATETIME NOT NULL,
    CONSTRAINT FK_Prestamos_Estudiantes FOREIGN KEY (IdEstudiante) REFERENCES Estudiantes(IdEstudiante),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (IdLibro) REFERENCES Libros(IdLibro)
);

-- Tabla Devoluciones
CREATE TABLE Devoluciones (
    IdDevolucion INT IDENTITY(1,1) PRIMARY KEY,
    IdPrestamo INT NOT NULL UNIQUE,
    FechaDevolucion DATETIME NOT NULL DEFAULT GETDATE(),
    Observaciones NVARCHAR(250) NULL,
    CONSTRAINT FK_Devoluciones_Prestamos FOREIGN KEY (IdPrestamo) REFERENCES Prestamos(IdPrestamo)
);
go

--INSERT
-- ========================================================
-- INSERTAR ESTUDIANTES
-- ========================================================
use BibliotecaDb;
INSERT INTO Estudiantes (Nombre, Matricula, Email)
VALUES 
('Carlos Mendoza', 'EST-2026-001', 'carlos.mendoza@universidad.edu'),
('Laura Gómez', 'EST-2026-002', 'laura.gomez@universidad.edu'),
('Javier Santos', 'EST-2026-003', 'javier.santos@universidad.edu'),
('Ana Paulina Rivas', 'EST-2026-004', 'ana.rivas@universidad.edu'),
('Marcos Peña', 'EST-2026-005', 'marcos.pena@universidad.edu'),
('Sofía Almonte', 'EST-2026-006', 'sofia.almonte@universidad.edu'),
('Diego Fernández', 'EST-2026-007', 'diego.fernandez@universidad.edu'),
('Valeria Rosario', 'EST-2026-008', 'valeria.rosario@universidad.edu'),
('Gabriel Taveras', 'EST-2026-009', 'gabriel.taveras@universidad.edu'),
('Camila Vargas', 'EST-2026-010', 'camila.vargas@universidad.edu');
GO
-- ========================================================
-- INSERTAR LIBROS
-- ========================================================
use BibliotecaDb;
INSERT INTO Libros (Titulo, Autor, Stock)
VALUES 
('Cien años de soledad', 'Gabriel García Márquez', 5),
('Don Quijote de la Mancha', 'Miguel de Cervantes', 3),
('Clean Code: A Handbook of Agile Software Craftsmanship', 'Robert C. Martin', 8),
('Design Patterns: Elements of Reusable Object-Oriented Software', 'Erich Gamma et al.', 4),
('El principito', 'Antoine de Saint-Exupéry', 10),
('Crónica de una muerte anunciada', 'Gabriel García Márquez', 6),
('El alquimista', 'Paulo Coelho', 7),
('Head First Design Patterns', 'Eric Freeman', 3),
('SQL Antipatterns: Avoiding the Pitfalls of Database Programming', 'Bill Karwin', 5),
('Domain-Driven Design: Tackling Complexity in the Heart of Software', 'Eric Evans', 2);
GO
-- ========================================================
-- INSERTAR PRÉSTAMOS (Asocia los Estudiantes 1 al 10 con los Libros)
-- ========================================================
use BibliotecaDb;
INSERT INTO Prestamos (IdEstudiante, IdLibro, FechaPrestamo, FechaDevolucionEsperada)
VALUES 
(1, 1, '2026-10-01 08:30:00', '2026-10-15 18:00:00'),
(2, 2, '2026-10-02 09:15:00', '2026-10-16 18:00:00'),
(3, 3, '2026-10-03 10:00:00', '2026-10-17 18:00:00'),
(4, 4, '2026-10-04 11:20:00', '2026-10-18 18:00:00'),
(5, 5, '2026-10-05 14:00:00', '2026-10-19 18:00:00'),
(6, 6, '2026-10-06 15:45:00', '2026-10-20 18:00:00'),
(7, 7, '2026-10-07 16:10:00', '2026-10-21 18:00:00'),
(8, 8, '2026-10-08 09:50:00', '2026-10-22 18:00:00'),
(9, 9, '2026-10-09 13:30:00', '2026-10-23 18:00:00'),
(10, 10, '2026-10-10 11:00:00', '2026-10-24 18:00:00');
GO
-- ========================================================
-- INSERTAR DEVOLUCIONES (Cada devolución vinculada a uno de los préstamos)
-- ========================================================
use BibliotecaDb;
INSERT INTO Devoluciones (IdPrestamo, FechaDevolucion, Observaciones)
VALUES 
(1, '2026-10-14 10:00:00', 'Entregado a tiempo y en óptimas condiciones.'),
(2, '2026-10-15 11:30:00', 'Devuelto sin novedades.'),
(3, '2026-10-16 14:20:00', 'Cubierta con ligero desgaste pero en buen estado.'),
(4, '2026-10-17 09:00:00', 'Entregado a tiempo.'),
(5, '2026-10-18 16:40:00', 'Páginas limpias, todo correcto.'),
(6, '2026-10-19 12:15:00', 'Entregado antes de la fecha esperada.'),
(7, '2026-10-20 08:45:00', 'Entregado en perfecto estado.'),
(8, '2026-10-21 17:10:00', 'Sin daños físicos reportados.'),
(9, '2026-10-22 15:30:00', 'Devolución completa dentro del plazo.'),
(10, '2026-10-23 13:00:00', 'Libro devuelto en condición impecable.');
GO