use BibliotecaDb;
-- Tabla Estudiantes
CREATE TABLE Estudiantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Matricula NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL
);

-- Tabla Libros
CREATE TABLE Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(150) NOT NULL,
    Autor NVARCHAR(100) NOT NULL,
    Stock INT NOT NULL DEFAULT 1
);

-- Tabla Prestamos
CREATE TABLE Prestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EstudianteId INT NOT NULL,
    LibroId INT NOT NULL,
    FechaPrestamo DATETIME NOT NULL DEFAULT GETDATE(),
    FechaDevolucionEsperada DATETIME NOT NULL,
    CONSTRAINT FK_Prestamos_Estudiantes FOREIGN KEY (EstudianteId) REFERENCES Estudiantes(Id),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (LibroId) REFERENCES Libros(Id)
);

-- Tabla Devoluciones
CREATE TABLE Devoluciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PrestamoId INT NOT NULL UNIQUE,
    FechaDevolucion DATETIME NOT NULL DEFAULT GETDATE(),
    Observaciones NVARCHAR(200),
    CONSTRAINT FK_Devoluciones_Prestamos FOREIGN KEY (PrestamoId) REFERENCES Prestamos(Id)
);
go