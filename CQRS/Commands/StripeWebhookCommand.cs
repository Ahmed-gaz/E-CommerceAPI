using E_CommerceAPI.Models;
using MediatR;
using Stripe.Climate;

namespace E_CommerceAPI.CQRS.Commands
{
    public class StripeWebhookCommand : IRequest<Payment?>
    {
        public string Json { get; set; }
        public string SignatureHeader { get; set; }

        public StripeWebhookCommand(string json, string signatureHeader)
        {
            Json = json;
            SignatureHeader = signatureHeader;
        }
    }

}
