using eShopOnTelegram.ApplicationContent.Keys;

using Newtonsoft.Json;

namespace eShopOnTelegram.ApplicationContent.Models;

public class ApplicationContentModel
{
    // TelegramBot properties

    [JsonProperty(PropertyName = ApplicationContentKey.TelegramBot.StartError)]
    public required string TelegramBot_StartError { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.TelegramBot.WelcomeText)]
    public required string TelegramBot_WelcomeText { get; set; }

    
    [JsonProperty(PropertyName = ApplicationContentKey.TelegramBot.OpenShopButtonText)]
    public required string TelegramBot_OpenShopButtonText { get; set; }

    
    [JsonProperty(PropertyName = ApplicationContentKey.TelegramBot.UnknownCommandText)]
    public required string TelegramBot_UnknownCommandText { get; set; }

    
    [JsonProperty(PropertyName = ApplicationContentKey.TelegramBot.DefaultErrorMessage)]
    public required string TelegramBot_DefaultErrorMessage { get; set; }

    
    // Order properties

    [JsonProperty(PropertyName = ApplicationContentKey.Order.ShowUnpaidOrder)]
    public required string Order_ShowUnpaidOrder { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.NoUnpaidOrderFound)]
    public required string Order_NoUnpaidOrderFound { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.AlreadyPaidOrExpired)]
    public required string Order_AlreadyPaidOrExpired { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.CreateErrorMessage)]
    public required string Order_CreateErrorMessage { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage)]
    public required string Order_InvoiceGenerationFailedErrorMessage { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.OrderNumberTitle)]
    public required string Order_OrderNumberTitle { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Order.OrderSummaryTitle)]
    public required string Order_OrderSummaryTitle { get; set; }

    
    [JsonProperty(PropertyName = ApplicationContentKey.Order.TotalPriceTitle)]
    public required string Order_TotalPriceTitle { get; set; }

    
    // Payment properties

    [JsonProperty(PropertyName = ApplicationContentKey.Payment.ChoosePaymentMethod)]
    public required string Payment_ChoosePaymentMethod { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Payment.NoEnabledPayments)]
    public required string Payment_NoEnabledPayments { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Payment.ProceedToPayment)]
    public required string PaymentProceedToPayment { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Payment.InvoiceReceiveMessage)]
    public required string Payment_InvoiceReceiveMessage { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Payment.SuccessfullPayment)]
    public required string Payment_SuccessfullPayment { get; set; }


    [JsonProperty(PropertyName = ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation)]
    public required string Payment_ErrorDuringPaymentConfirmation { get; set; }
}