using SweetBakeryApp.Models;
using SweetBakeryApp.Services;

MenuService menuService = new();
CashierService cashierService = new();
bool isRunning = true;

while (isRunning)
{
    Console.WriteLine("\n=== SWEET BAKERY: BUKU MENU TOKO ===");
    Console.WriteLine("1. Lihat Katalog Menu");
    Console.WriteLine("2. Tambah Menu Baru");
    Console.WriteLine("0. Keluar");
    Console.Write("Pilih menu: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("\n--- DAFTAR MENU SAAT INI ---");
            var currentMenu = menuService.GetAllMenu();

            if (currentMenu.Count == 0)
            {
                Console.WriteLine("Katalog menu masih kosong.");
                break;
            }

            foreach (var item in currentMenu)
            {
                string shortCode = item.Id.ToString()[..8].ToUpper(); // C# Range Operator [..8] menggantikan Substring(0, 8)
                Console.WriteLine($"[{shortCode}] {item.Name,-20} | Rp{item.Price:N0}");
            }
            break;

        case "2":
            Console.WriteLine("\n--- TAMBAH MENU BARU ---");
            Console.Write("Masukkan Nama Kue: ");
            string? name = Console.ReadLine();

            Console.Write("Masukkan Harga (Rp): ");
            string? priceInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name) && decimal.TryParse(priceInput, out decimal price))
            {
                menuService.AddNewMenu(name, price);
                Console.WriteLine("✓ Menu baru berhasil didaftarkan ke etalase!");
            }
            else
            {
                Console.WriteLine("✗ Gagal: Nama tidak boleh kosong dan harga harus berupa angka valid.");
            }
            break;

        case "3":
            Console.WriteLine("\n--- TRANSAKSI KASIR: BUAT PESANAN ---");
            Console.Write("Masukkan Nama Kue atau Kode Menu: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("✗ Gagal: Input tidak boleh kosong.");
                break;
            }

            // Panggil fungsi pencarian fleksibel
            MenuItem? selectedItem = menuService.FindMenu(input);
            if (selectedItem == null)
            {
                Console.WriteLine($"✗ Menu dengan kata kunci \"{input}\" tidak ditemukan!");
                break;
            }

            Console.WriteLine($"Menu terpilih: {selectedItem.Name} (Rp{selectedItem.Price:N0})");
            Console.Write($"Jumlah pesanan: ");
            string? qtyInput = Console.ReadLine();

            if (int.TryParse(qtyInput, out int qty) && qty > 0)
            {
                var ticket = cashierService.CreateOrder(selectedItem, qty);
                Console.WriteLine("\n✓ Pesanan berhasil dibuat!");
                Console.WriteLine($"Tiket ID : {ticket.Id.ToString()[..8].ToUpper()}");
                Console.WriteLine($"Item     : {ticket.Item.Name} x{ticket.Quantity}");
                Console.WriteLine($"Total    : Rp{ticket.TotalPrice:N0}");
                Console.WriteLine($"Status   : {ticket.Status}");
            }
            else
            {
                Console.WriteLine("✗ Gagal: Jumlah pesanan harus berupa angka bulat positif.");
            }
            break;

        case "0":
            isRunning = false;
            Console.WriteLine("Toko ditutup. Sampai jumpa!");
            break;

        default:
            Console.WriteLine("Pilihan tidak valid.");
            break;
    }
}