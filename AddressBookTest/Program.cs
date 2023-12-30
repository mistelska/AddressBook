using AddressBookTest.Services;

IMenuService menuService = new MenuService();
menuService.ShowMenu(); // Allt ligger i ShowMenu, för att i Program ska det ligga så lite kod som möjligt
