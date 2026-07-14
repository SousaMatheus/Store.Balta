using Store.Balta.Domain.Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Order : Entity
{
    public Order(Customer customer, Discount discount, decimal deliveryFee)
    {
        Customer = customer;
        Discount = discount;
        DeliveryFee = deliveryFee;
        Date = DateTime.Now;
        Number = Guid.NewGuid().ToString().Substring(0, 8);
        Status = EOrdemStatus.WaitingPayment;
        Items = new List<OrderItem>();
    }

    public Customer Customer { get; private set; }
    public DateTime Date { get; private set; }
    public string Number { get; private set; }
    public IList<OrderItem> Items { get; private set; }
    public EOrdemStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public Discount Discount { get; private set; }
    public decimal DeliveryFee { get; private set; }

    public void AddItens(Product product, int quantity)
    {
        var item = new OrderItem(product, quantity);
        Items.Add(item);
    }

    public decimal Total()
    {
        decimal total = Items.Sum(item => item.Total());

        total += DeliveryFee;
        total -= Discount != null ? Discount.Value() : 0;

        return total;
    }

    public void Pay(decimal amount)
    {
        if (amount == Total())
            Status = EOrdemStatus.WaitingDelivery;
    }

    public void Cancel()
    {
        Status = EOrdemStatus.Cancelled;
    }
}