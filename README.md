# HotelManagementSystem 

## **Project Overview**

**HotelManagementSystem** is a hotel management system with functionalities for employees and guests. The system includes reservation management, room categorization, and guest management. The application supports three themes (dark, light, blue) and two languages (English, Serbian).

## **Projects**

1. **Rooms Management:** Manage room details, including capacity, amenities, and pricing.
2. **Reservations Management:** Handle guest reservations and room bookings.
3. **Guests Management:** Maintain guest information and profiles.
4. **Bill Generation:** Generate bills for guest stays.

## **Tech Stack**

- **.NET 6:** Core framework for building and running the application.
- **C#:** Programming language used for development.
- **WPF (Windows Presentation Foundation):** For creating the graphical user interface.
- **MySQL:** Database for storing application data.
- **MaterialDesignThemes:** For enhancing the UI/UX.

### Usage

### Setup

1. **Clone the Repository**
    ```sh
    git clone https://github.com/andjeladragosavljevic/ProjekatB
    cd HotelManagementSystem
    ```

2. **Install Dependencies**
    Ensure you have .NET 6 SDK installed. Restore NuGet packages using:
    ```sh
    dotnet restore
    ```

3. **Build the Solution**
    ```sh
    dotnet build
    ```

### Run the Application

1. **Start the Application**
    ```sh
    dotnet run --project ProjekatB
    ```

### Example Usage

#### Room Management

- **Add Room**
    - **Function**: Add new rooms with details such as name, capacity, amenities, description, price, and category.
    - **Example**: 
    ```sh
    Create room with the above parameters using the GUI.
    ```

#### Reservation Management

- **Make a Reservation**
    - **Function**: Book a room for a guest with check-in and check-out dates.
    - **Example**: 
    ```sh
    Create reservation with the above parameters using the GUI.
    ```

#### Bill Generation

- **Generate Bill**
    - **Function**: Generate and print bills for guest stays.
    - **Example**: 
    ```sh
    Generate bill with the above parameters using the GUI.
    ```

## **Screenshots**

![Main Window](https://github.com/andjeladragosavljevic/ProjekatB/blob/master/MainWindow.png)
![Settings Menu](https://github.com/andjeladragosavljevic/ProjekatB/blob/master/SettingsMenu.png)
![Login](https://github.com/andjeladragosavljevic/ProjekatB/blob/master/Login.png)
