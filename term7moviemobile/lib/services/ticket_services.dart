import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/services/api.dart';

class TicketServices {
  static Future<List<TicketModel>> getTickets(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/tickets', queryParameters: queryParams);
    return List<TicketModel>.from(res.data['result']['results'].map((x) => TicketModel.fromJson(x)));
  }

  static Future<List<TicketModel>> getSaleTickets() async {
    var res = await Api().api.get('/tickets/sales');
    print(res);
    return List<TicketModel>.from(res.data['result'].map((x) => TicketModel.fromJson(x)));
  }
}