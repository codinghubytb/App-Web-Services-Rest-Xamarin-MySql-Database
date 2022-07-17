
/*run the commands one by one if there is a problem*/

create database db_school;


CREATE TABLE db_school.classroom
(
	classroomId int not null auto_increment,
	classroomName varchar(100) not null,
	classroomCreated datetime not null,
	classroomModified datetime not null,
	classroomNbPerson int,
	primary key (classroomId)	
)


CREATE TABLE db_school.student
(
	studentId int not null auto_increment,
	classroomId int not null,
	studentName varchar(100) not null,
	studentCreated datetime not null,
	studentModified datetime not null,
	studentAge int,
	studentChatty bool,
	primary key (studentId)	
)

INSERT INTO db_school.classroom (classroomName,classroomCreated,classroomModified,classroomNbPerson) VALUES
	 ('1st',now(),now(),0),
	 ('2th',now(),now(),0),
	 ('3th',now(),now(),0),
	 ('4th',now(),now(),0);