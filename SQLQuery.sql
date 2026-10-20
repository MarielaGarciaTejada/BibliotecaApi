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