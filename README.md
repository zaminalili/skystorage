## SkyStorage API
SkyStorage is a cloud-based file storage and management system designed to securely store and manage files. The project is built using .NET 8.0 Web API and follows Clean Architecture principles.

## Technologies Used
* **ASP.NET Core Web API:** Backend services and API
* **Azure Blob Storage:** Main storage provider for files
* **MS SQL:** Database for file metadata storage
* **Entity Framework Core:** ORM framework
* **MediatR and CQRS Pattern:** For separating commands and queries and creating a clean code structure
* **Microsoft Identity:** For user authentication and authorization

## Project structure
* `src/SkyStorage.Domain:` Core project logic and entities
* `src/SkyStorage.Application:` Business logic, CQRS patterns, and Data Transfer Objects (DTOs)
* `src/SkyStorage.Infrastructure:` Data storage, external services and other dependencies
* `src/SkyStorage.Presentation:` API controllers and endpoints
* `Tests/*:` Unit tests for project

## Setup
To set up project locally, follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/zaminalili/skystorage.git
   ```
2. Configure Azure Blob Storage and MS SQL settings in the `appsettings.Development.json` file.
3. Run the project using:
   ```
   dotnet run
   ```
> Make sure to set up your Azure and SQL configurations properly for integration.

## API endpoints

* **`GET /api/users/{userId}/files`**
  - Parameters: Guid `userId`, string? `searchPhrase`, int `pageSize` = 10, int `pageNumber` = 1


* **`POST /api/users/{userId}/files`**
  - Parameters: Guid `userId`
  - Body: binary `File`

* **`GET /api/users/{userId}/files/{fileId}/download`**
  - Parameters: Guid `userId`, Guid `fileId`

* **`DELETE /api/users/{userId}/files/{fileId}`**
  - Parameters: Guid `userId`, Guid `fileId`

* **`POST /api/identity/register`**
  - Body: JSON 
`` 
{
  "email": "string",
  "password": "string"
} 
``
  - Anonymous access

* **`POST /api/identity/login`**
  - Parameters: boolean? useCookies, boolean? useSessionCookies
  - Body: JSON
    ``
    {
      "email": "string",
      "password": "string",
      "twoFactorCode": "string",
      "twoFactorRecoveryCode": "string"
    }
    ``
  - Anonymous access

* **`GET /api/identity/currentUser`**

* **`POST /api/identity/refresh`**
  - Body: JSON
  ``
  {
    "refreshToken": "string"
  }
  ``

* **`GET /api/identity/confirmEmail`**
  - Parameters: Guid `userId`, string `code`, string `changedEmail`

* **`POST /api/identity/resendConfirmationEmail`**
  - Body: JSON
  ``
  {
    "email": "string"
  }
  ``

* **`POST /api/identity/forgotPassword`**
  - Body: JSON
  ``
  {
    "email": "string"
  }
  ``
  - Anonymous access

* **`POST /api/identity/resetPassword`**
  - Body: JSON
  ``
  {
    "email": "string",
    "resetCode": "string",
    "newPassword": "string"
  }
  ``

* **`POST /api/identity/manage/2fa`**
  - Body: JSON
  ``
  {
    "enable": true,
    "twoFactorCode": "string",
    "resetSharedKey": true,
    "resetRecoveryCodes": true,
    "forgetMachine": true
  }
  ``

* **`GET /api/identity/manage/info`**

* **`POST /api/identity/manage/info`**
  - Body: JSON
``
{
  "newEmail": "string",
  "newPassword": "string",
  "oldPassword": "string"
}
``


> Authorization: Bearer token
