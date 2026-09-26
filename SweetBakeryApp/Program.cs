using SweetBakeryApp.Data;
using SweetBakeryApp.Models;
using SweetBakeryApp.Services;

using BakeryDbContext dbContext = new();

MenuService menuService = new(dbContext);
CashierService cashierService = new(dbContext);
KitchenService kitchenService = new(dbContext);

menuService.SeedInitialMenu();
Console.OutputEncoding = System.Text.Encoding.UTF8;
bool isRunning = true;

while (isRunning)
{
    Console.WriteLine("\n=== SWEET BAKERY: BUKU MENU TOKO ===");
    Console.WriteLine("1. Lihat Katalog Menu");
    Console.WriteLine("2. Tambah Menu Baru (Admin)");
    Console.WriteLine("3. Buat Pesanan Baru (Kasir)");
    Console.WriteLine("4. Lihat Daftar Antrian Pesanan");
    Console.WriteLine("5. Proses Pesanan (Dapur)");
    Console.WriteLine("6. Lihat Produk Siap Saji");
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
        case "4":
            Console.WriteLine("\n--- ANTREAN PESANAN AKTIF ---");
            var orders = cashierService.GetActiveOrders();

            if (orders.Count == 0)
            {
                Console.WriteLine("Belum ada pesanan aktif.");
                break;
            }

            foreach (var order in orders)
            {
                string ticketCode = order.Id.ToString()[..8].ToUpper();
                Console.WriteLine($"[{ticketCode}] {order.Item.Name} x{order.Quantity} | Rp{order.TotalPrice:N0} | Status: {order.Status}");
            }
            break;
        case "5":
            Console.WriteLine("\n--- DAPUR: PROSES PESANAN ---");
            var pendingOrders = cashierService.GetActiveOrders()
                .Where(o => o.Status == OrderStatus.Pending)
                .ToList();

            if (pendingOrders.Count == 0)
            {
                Console.WriteLine("Tidak ada antrean pesanan yang perlu dimasak.");
                break;
            }

            Console.WriteLine("Pilih tiket pesanan:");
            for (int i = 0; i < pendingOrders.Count; i++)
            {
                var ord = pendingOrders[i];
                string ticketId = ord.Id.ToString()[..8].ToUpper();
                Console.WriteLine($"{i + 1}. [{ticketId}] {ord.Item.Name} x{ord.Quantity}");
            }

            Console.Write("Pilih nomor antrean: ");
            if (int.TryParse(Console.ReadLine(), out int orderIndex) && orderIndex >= 1 && orderIndex <= pendingOrders.Count)
            {
                var chosenTicket = pendingOrders[orderIndex - 1];

                Console.WriteLine("\nPilih Metode Pengolahan:");
                Console.WriteLine("1. Panggang Oven (BakeInOven)");
                Console.WriteLine("2. Kukus (Steam)");
                Console.WriteLine("3. Dinginkan Kulkas (Chill)");
                Console.Write("Metode: ");

                if (Enum.TryParse(Console.ReadLine(), out CookingMethod method) && Enum.IsDefined(method))
                {
                    var result = kitchenService.CookOrder(chosenTicket, method);

                    Console.WriteLine("\n[Simulasi Dapur: Meracik Bahan Baku]");
                    foreach (var ing in result.UsedIngredients)
                    {
                        Console.WriteLine($"-> Mengambil {ing.Name}: {ing.AmountInGrams} gram");
                    }

                    Console.WriteLine($"\n✓ Sukses! Produk \"{result.Product.Name}\" selesai diproses.");
                    Console.WriteLine($"Metode  : {result.Product.Method}");
                    Console.WriteLine($"Waktu   : {result.Product.PreparedAt:HH:mm:ss}");
                    Console.WriteLine($"Status  : Tiket antrean berubah menjadi {chosenTicket.Status}");
                }
                else
                {
                    Console.WriteLine("✗ Metode memasak tidak valid.");
                }
            }
            else
            {
                Console.WriteLine("✗ Nomor antrean tidak valid.");
            }
            break;

        case "6":
            Console.WriteLine("--- DAFTAR PRODUK SIAP SAJI ---");
            var bakedList = kitchenService.GetReadyProducts();
            if (bakedList.Count == 0)
            {
                Console.WriteLine("Belum ada produk yang selesai dimasak.");
            }
            else
            {
                foreach (var p in bakedList)
                {
                    Console.WriteLine($"- {p.Name} | Metode: {p.Method} | Dimasak pada: {p.PreparedAt:HH:mm:ss}");
                }
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