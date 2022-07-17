import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/services/showtime_services.dart';
import 'package:term7moviemobile/services/ticket_services.dart';

class BookingController extends GetxController {
  String id = '';
  static BookingController instance = Get.find();
  RxBool isLoading = false.obs;
  List<TicketModel> tickets = <TicketModel>[];
  ShowtimeModel? showtime;
  RxList<TicketModel> selected = <TicketModel>[].obs;
  RxDouble total = (0.0).obs;
  int pageSize = 0;

  void fetchData() async {
    try{
      isLoading.value = true;
      id = Get.parameters['id']!;
      total.value = 0;
      selected.value = [];
      showtime = await ShowtimeServices.getShowtimeDetail(id);
      if (showtime?.room != null) {
        pageSize = showtime!.room!.numberOfColumn! * showtime!.room!.numberOfRow!;
        List<TicketModel> _data = await TicketServices.getTickets({'ShowtimeId': id,
          'PageSize': pageSize});
        tickets.assignAll(_data);
        print(tickets);
      }
      print(showtime);
    }finally{
      isLoading.value = false;
    }
  }

  void setSelected(var ticket) {
    if (selected.contains(ticket)) {
      selected.remove(ticket);
      total.value = total.value - ticket.sellingPrice!;
    } else {
      selected.add(ticket);
      total.value = total.value + ticket.sellingPrice!;
    }
  }
}