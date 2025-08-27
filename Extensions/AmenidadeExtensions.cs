using GerenciadorHotel.Models;

namespace GerenciadorHotel.Extensions
{
    public static class AmenidadeExtensions
    {
        public static string GetIconClass(this Amenidade amenidade)
        {
            if (amenidade?.Nome == null) return "fas fa-star";

            var nome = amenidade.Nome.ToLower().Trim();
            
            return nome switch
            {
                // Internet e Conectividade
                var n when n.Contains("wifi") || n.Contains("wi-fi") || n.Contains("internet") => "fas fa-wifi",
                
                // Entretenimento
                var n when n.Contains("tv") || n.Contains("televisão") || n.Contains("televisao") => "fas fa-tv",
                var n when n.Contains("netflix") || n.Contains("streaming") => "fas fa-play-circle",
                var n when n.Contains("cabo") || n.Contains("sky") => "fas fa-satellite-dish",
                
                // Climatização
                var n when n.Contains("ar condicionado") || n.Contains("ar-condicionado") || n.Contains("climatização") => "fas fa-snowflake",
                var n when n.Contains("ventilador") => "fas fa-fan",
                var n when n.Contains("aquecimento") || n.Contains("calefação") => "fas fa-thermometer-three-quarters",
                
                // Alimentação e Bebidas
                var n when n.Contains("frigobar") || n.Contains("mini bar") || n.Contains("minibar") => "fas fa-wine-bottle",
                var n when n.Contains("café") || n.Contains("cafeteira") => "fas fa-coffee",
                var n when n.Contains("água") || n.Contains("agua") => "fas fa-tint",
                var n when n.Contains("geladeira") || n.Contains("refrigerador") => "fas fa-cube",
                var n when n.Contains("microondas") => "fas fa-microchip",
                
                // Banheiro e Higiene
                var n when n.Contains("banheira") => "fas fa-bath",
                var n when n.Contains("chuveiro") || n.Contains("ducha") => "fas fa-shower",
                var n when n.Contains("secador") || n.Contains("hair dryer") => "fas fa-wind",
                var n when n.Contains("toalha") || n.Contains("roupão") => "fas fa-tshirt",
                var n when n.Contains("amenities") || n.Contains("produtos de higiene") => "fas fa-soap",
                
                // Conforto
                var n when n.Contains("cofre") || n.Contains("safe") => "fas fa-lock",
                var n when n.Contains("ferro") || n.Contains("passadeira") => "fas fa-tshirt",
                var n when n.Contains("mesa") || n.Contains("escrivaninha") => "fas fa-desk",
                var n when n.Contains("cadeira") => "fas fa-chair",
                var n when n.Contains("armário") || n.Contains("guarda-roupa") => "fas fa-door-open",
                
                // Comodidades Premium
                var n when n.Contains("jacuzzi") || n.Contains("spa") => "fas fa-hot-tub",
                var n when n.Contains("varanda") || n.Contains("sacada") => "fas fa-mountain",
                var n when n.Contains("vista") => "fas fa-eye",
                var n when n.Contains("piscina") => "fas fa-swimming-pool",
                
                // Serviços
                var n when n.Contains("room service") || n.Contains("serviço de quarto") => "fas fa-concierge-bell",
                var n when n.Contains("lavanderia") || n.Contains("laundry") => "fas fa-tshirt",
                var n when n.Contains("estacionamento") || n.Contains("parking") => "fas fa-parking",
                var n when n.Contains("academia") || n.Contains("fitness") => "fas fa-dumbbell",
                
                // Comunicação
                var n when n.Contains("telefone") || n.Contains("phone") => "fas fa-phone",
                
                // Padrão
                _ => "fas fa-star"
            };
        }

        public static string GetDescription(this Amenidade amenidade)
        {
            return amenidade?.Descricao ?? amenidade?.Nome ?? "Amenidade";
        }
    }
}
