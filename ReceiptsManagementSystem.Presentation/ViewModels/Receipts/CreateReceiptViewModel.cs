using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;
using ReceiptsManagementSystem.Domain.Enums;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;

namespace ReceiptsManagementSystem.Presentation.ViewModels.Receipts;

public sealed partial class CreateReceiptViewModel : BaseViewModel
{
    private readonly IMediator _mediator;

    // --- Campos del formulario ---
    [ObservableProperty] private string _customerName = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private decimal _amount;
    [ObservableProperty] private string _currency = "GTQ";
    [ObservableProperty] private PaymentMethod _paymentMethod = PaymentMethod.Cash;
    [ObservableProperty] private string _checkOrTransferNumber = string.Empty;
    [ObservableProperty] private string _accountNumber = string.Empty;
    [ObservableProperty] private string _bank = string.Empty;
    [ObservableProperty] private string _customerSignatureName = string.Empty;
    [ObservableProperty] private string _receiverName = string.Empty;

    // --- Estado condicional según método de pago ---
    [ObservableProperty] private bool _showCheckFields;
    [ObservableProperty] private bool _showTransferFields;

    // --- Feedback al usuario ---
    [ObservableProperty] private string _successMessage = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public static IEnumerable<PaymentMethod> PaymentMethods =>
        Enum.GetValues<PaymentMethod>();

    public CreateReceiptViewModel(IMediator mediator)
    {
        _mediator = mediator;
        Title = "Nuevo Recibo";
    }

    /// <summary>
    /// Cuando cambia el método de pago, actualiza la visibilidad
    /// de los campos condicionales (cheque/transferencia).
    /// </summary>

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed",
        Justification = "Used by MVVM Toolkit source generator")]

    partial void OnPaymentMethodChanged(PaymentMethod value)
    {
        ShowCheckFields = value == PaymentMethod.Check;
        ShowTransferFields = value == PaymentMethod.Transfer;

        // Limpiar campos que no aplican
        if (value == PaymentMethod.Cash)
        {
            CheckOrTransferNumber = string.Empty;
            AccountNumber = string.Empty;
            Bank = string.Empty;
        }
    }

    [RelayCommand]
    private async Task SaveReceiptAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;

            if (!ValidateForm())
            {
                return;
            }

            try
            {
                // Construir el DTO con los datos del formulario
                var dto = new CreateReceiptDto
                {
                    CustomerId = Guid.NewGuid(), // temporal hasta tener gestión de clientes
                    CustomerName = CustomerName,
                    Amount = Amount,
                    Currency = Currency,
                    Description = Description,
                    PaymentMethod = PaymentMethod,
                    CheckOrTransferNumber = string.IsNullOrWhiteSpace(CheckOrTransferNumber)
                        ? null
                        : CheckOrTransferNumber,
                    AccountNumber = string.IsNullOrWhiteSpace(AccountNumber)
                        ? null
                        : AccountNumber,
                    Bank = string.IsNullOrWhiteSpace(Bank)
                        ? null
                        : Bank,
                    CustomerSignatureName = CustomerSignatureName,
                    ReceiverName = ReceiverName
                };

                // Enviar comando para crear el recibo
                var command = new CreateReceiptCommand(dto);
                var receiptId = await _mediator.Send(command, CancellationToken.None);

                SuccessMessage = $"Recibo creado exitosamente. ID: {receiptId}";
                ClearForm();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al crear el recibo: {ex.Message}";
            }
        });
    }

    [RelayCommand]
    private void ClearForm()
    {
        CustomerName = string.Empty;
        Description = string.Empty;
        Amount = 0;
        PaymentMethod = PaymentMethod.Cash;
        CheckOrTransferNumber = string.Empty;
        AccountNumber = string.Empty;
        Bank = string.Empty;
        CustomerSignatureName = string.Empty;
        ReceiverName = string.Empty;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S2325:Methods should be static",
        Justification = "Depends on ViewModel state and MVVM architecture")]

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            ErrorMessage = "El nombre del cliente es requerido.";
            return false;
        }

        if (Amount <= 0)
        {
            ErrorMessage = "El monto debe ser mayor a cero.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ErrorMessage = "El concepto es requerido.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(CustomerSignatureName))
        {
            ErrorMessage = "El nombre de quien entrega es requerido.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ReceiverName))
        {
            ErrorMessage = "El nombre de quien recibe es requerido.";
            return false;
        }

        if ((PaymentMethod == PaymentMethod.Check || PaymentMethod == PaymentMethod.Transfer)
            && string.IsNullOrWhiteSpace(CheckOrTransferNumber))
        {
            ErrorMessage = "El número de cheque/transferencia es requerido.";
            return false;
        }

        if (PaymentMethod == PaymentMethod.Transfer
            && (string.IsNullOrWhiteSpace(AccountNumber) || string.IsNullOrWhiteSpace(Bank)))
        {
            ErrorMessage = "Banco y número de cuenta son requeridos para transferencia.";
            return false;
        }

        return true;
    }
}
