--Генерация базы собрана в один DDL скрипт
--Создаем базу
USE master
CREATE DATABASE PV_521_DDL_HW ON (
    NAME=PV_521_DDL_HW,
    FILENAME='E:\Microsoft SQL Server\MSSQL16.TEST\MSSQL\DATA\PV_521_DDL_HW.mdf',
    SIZE=8 MB,
    MAXSIZE=500 MB,
    FILEGROWTH=8 MB
) LOG ON (
    NAME=PV_521_DDL_HW_Log,
    FILENAME='E:\Microsoft SQL Server\MSSQL16.TEST\MSSQL\DATA\PV_521_DDL_HW_Log.ldf',
    SIZE=8 MB,
    MAXSIZE=500 MB,
    FILEGROWTH=8 MB
)
GO
--Иначе всю базу затолкаем не по адресу  :)
USE PV_521_DDL_HW
GO
--Создаем таблицы сущностей
CREATE TABLE Directions
(
    direction_id tinyint PRIMARY KEY,
    direction_name NVARCHAR(150) NOT NULL,
)
CREATE TABLE Groups
(
    group_id INT PRIMARY KEY,
    group_name NVARCHAR(24) NOT NULL,
    direction tinyint NOT NULL CONSTRAINT FK_Groups_Direction FOREIGN KEY REFERENCES Directions (direction_id)
)
CREATE TABLE Students
(
    student_id INT PRIMARY KEY identity(1, 1),
    last_name NVARCHAR(50) NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    middle_name NVARCHAR(50),
    birth_date DATE NOT NULL,
    [group] INT NOT NULL --group - является зарезервированным словом в SQL, 
        -- названия полей, совпадающих с зарезервированными, нужно заключать в [] 
        CONSTRAINT FK_Students_Group FOREIGN KEY REFERENCES Groups (group_id),
)
CREATE TABLE Teachers
(
    teacher_id INT PRIMARY KEY,
    last_name NVARCHAR(50) NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    middle_name NVARCHAR(50),
    birth_date DATE NOT NULL,
)
CREATE TABLE Disciplines
(
    discipline_id SMALLINT PRIMARY KEY,
    discipline_name NVARCHAR(250) NOT NULL,
    number_of_lessons tinyint NOT NULL,
)
CREATE TABLE DisciplinesDirectionsRelation
(
    discipline SMALLINT,
    direction tinyint,
    PRIMARY KEY (discipline, direction),
    CONSTRAINT FK_DDR_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id),
    CONSTRAINT FK_DDR_Direction FOREIGN KEY (direction) REFERENCES Directions (direction_id)
)
CREATE TABLE TeachersDisciplinesRelation
(
    teacher INT,
    discipline SMALLINT,
    PRIMARY KEY (teacher, discipline),
    CONSTRAINT FK_TDR_Teacher FOREIGN KEY (teacher) REFERENCES Teachers (teacher_id),
    CONSTRAINT FK_TDR_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id)
)
CREATE TABLE RequiredDisciplines
(
    discipline SMALLINT,
    required_discipline SMALLINT,
    PRIMARY KEY (discipline, required_discipline),
    CONSTRAINT FK_RD_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id),
    CONSTRAINT FK_RD_Required FOREIGN KEY (required_discipline) REFERENCES Disciplines (discipline_id),
)
CREATE TABLE DependentDisciplines
(
    discipline SMALLINT,
    dependent_discipline SMALLINT,
    PRIMARY KEY (discipline, dependent_discipline),
    CONSTRAINT FK_DD_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id),
    CONSTRAINT FK_DD_Depentent FOREIGN KEY (dependent_discipline) REFERENCES Disciplines (discipline_id),
)
-- Примерно тут начинается домашка
CREATE TABLE Schedule
(
    lesson_id BIGINT IDENTITY(1, 1),
    [date] DATE NOT NULL,
    [time] TIME(0) NOT NULL,
    [group] INT NOT NULL,
    discipline SMALLINT NOT NULL,
    teacher INT NOT NULL,
    subject NVARCHAR(256) NOT NULL,
    [status] BIT NOT NULL,
    -- да, так тоже можно, имена для первичных ключей лишними не бывают
    CONSTRAINT PK_Lesson PRIMARY KEY (lesson_id),
    CONSTRAINT FK_Schedule_Groups FOREIGN KEY ([group]) REFERENCES Groups (group_id),
    CONSTRAINT FK_Schedule_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id),
    CONSTRAINT FK_Schedule_Teachers FOREIGN KEY (teacher) REFERENCES Teachers (teacher_id)
)
CREATE TABLE Grades
(
    student INT NOT NULL,
    lesson BIGINT NOT NULL,
    grade_1 TINYINT,
    grade_2 TINYINT,
    CONSTRAINT PK_Grades PRIMARY KEY (student, lesson),
    CONSTRAINT FK_Grades_Lesson FOREIGN KEY (lesson) REFERENCES Schedule (lesson_id),
    CONSTRAINT FK_Grades_Student FOREIGN KEY (student) REFERENCES Students (student_id),
    CONSTRAINT CK_Grade_1 CHECK (
        grade_1>0
        AND grade_1<=12
    ),
    CONSTRAINT CK_Grade_2 CHECK (
        grade_2>0
        AND grade_2<=12
    )
)
CREATE TABLE Exams
(
    student INT NOT NULL,
    discipline SMALLINT NOT NULL,
    grade TINYINT,
    CONSTRAINT PK_Exams PRIMARY KEY (student, discipline),
    CONSTRAINT FK_Exams_Discipline FOREIGN KEY (discipline) REFERENCES Disciplines (discipline_id),
    CONSTRAINT FK_Exams_Student FOREIGN KEY (student) REFERENCES Students (student_id),
    CONSTRAINT CK_ExamGrade CHECK (
        grade>0
        AND grade<=12
    )
)
CREATE TABLE HomeWorks
(
    [group] INT NOT NULL,
    lesson BIGINT NOT NULL,
    [data] VARBINARY(MAX),
    comment NVARCHAR(1024),
    deadline DATE,
    CONSTRAINT PK_HomeWorks PRIMARY KEY ([group], lesson),
    CONSTRAINT FK_HW_Group FOREIGN KEY ([group]) REFERENCES Groups (group_id),
    CONSTRAINT FK_HW_Lesson FOREIGN KEY (lesson) REFERENCES Schedule (lesson_id),
    CONSTRAINT CK_Null_HW CHECK (
        [data] IS NOT NULL
        OR comment IS NOT NULL
    )
)
CREATE TABLE ResultsHW
(
    student INT NOT NULL,
    [group] INT NOT NULL,
    lesson BIGINT NOT NULL,
    result VARBINARY(MAX),
    comment NVARCHAR(1024),
    grade TINYINT,
    CONSTRAINT PK_Results PRIMARY KEY (student, [group], lesson),
    CONSTRAINT FK_RHW_Groups FOREIGN KEY ([group], lesson) REFERENCES HomeWorks ([group], lesson),
    CONSTRAINT FK_RHW_Students FOREIGN KEY (student) REFERENCES Students (student_id),
    CONSTRAINT CK_Result_Or_Comment CHECK (
        result IS NOT NULL
        OR comment IS NOT NULL
    ),
    CONSTRAINT CK_HWGrade CHECK (
        grade>0
        AND grade<=12
    )
);

-- use master
-- drop database PV_521_DDL_HW