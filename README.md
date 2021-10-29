# SRE Exercise

## The instructions of MySQL Configuration

### Create an Admin User for remote access

Get into container bash

```bash
docker exec -it mysql /bin/bash
```

Login MySQL Server with predefined root password

```bash
mysql -u root -p
```

Create an Admin User with DBA permissions

```sql
CREATE USER 'admin'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON *.* TO 'admin'@'%';
FLUSH PRIVILEGES;
```

P.s. The password is very weak because of exercise purpose, remember to use strong password in production.

### Import the sample DB into MySQL

Clone the sample DB git repository to local

```bash
git clone https://github.com/datacharmer/test_db.git
```

Import the sample DB

```bash
cd test_db
mysql -h 127.0.0.1 -u admin -p < employees.sql
```

Validate the sample DB

```bash
mysql -h 127.0.0.1 -u admin -p -t < test_employees_sha.sql
```

If the validation passes, the configuration is completed and the DB is ready.

## Reference

<https://github.com/smartclouds-exercise/Site-Reliability-Engineering#build-sample-laravel-php-framework-site>