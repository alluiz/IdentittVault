# Identitt Vault 1.0

An open-source .NET Core 5.0 vault system for store and manage passwords with hard security.

## Getting Started

First of all, clone the master branch at this repository to your hard disk. Then follow these steps:

### Docker required

You need to install **Docker** into your machine to be able to run the Identitt Vault System. Go to https://www.docker.com/products/docker-desktop for get more info about instalation guide.

### Setup Enviroment

The Identitt Vault System needs some variables to run. It will be used to security of your system cypher password.

Create some **environment variables** in your OS host:

- **MYSQL_ROOT_PASSWORD**: Must have the database ROOT user password. If null the default password is: *"default_password"*

- **IDENTITT_KEY**: Must have your secret key in BASE64 format to cypher generated user private key. Cannot be NULL.

- **IDENTITT_IV**: Must have some value to work with AES cypher algorithm. Cannot be NULL.

### Running

To run the application you need to open your terminal and run the follow command:

> docker-compose up --build

The command above will download and install all system's dependencies and run the MySQL database with the required relational schema.

### Test

To test the application just check on Postman or Curl the POST /api/users route on **port 59000** (it can be configured at docker-compose.yml file) to create your first user.

Example:

```
curl -k --location --request POST 'https://localhost:59000/api/users' --header 'Content-Type: application/json' --data-raw '{
    "name": "Test of system",
    "email": "test@test.com",
    "password": "#TestPassword1"
}'
```

## Colaborate

If you desire to colaborate with the app please contact me on al.luiz@live.com