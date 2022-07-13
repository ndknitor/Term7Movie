import 'package:get/get.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/services/ticket_services.dart';

class SaleTicketsController extends GetxController {
  RxBool isLoading = false.obs;
  List<TicketModel> tickets = <TicketModel>[];

  void fetchData() async {
    try {
      isLoading.value = true;
      List<TicketModel> _data = await TicketServices.getSaleTickets();
      tickets.assignAll(_data);
      print(tickets);
    } finally {
      isLoading.value = false;
    }
  }
}
