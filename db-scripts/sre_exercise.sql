DROP DATABASE IF EXISTS sre_exercise;
CREATE DATABASE IF NOT EXISTS sre_exercise;
USE sre_exercise;

DROP TABLE IF EXISTS employees;

CREATE TABLE employees (
    id         VARCHAR(16)     NOT NULL,
    email      VARCHAR(50)     NOT NULL,
    mobile     VARCHAR(10)     NOT NULL,
    title      VARCHAR(50)     NOT NULL,
    department VARCHAR(200)    NOT NULL,
    PRIMARY KEY (id)
);