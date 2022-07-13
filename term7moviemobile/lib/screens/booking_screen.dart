import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/booking_controller.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';

class BookingScreen extends StatelessWidget {
  const BookingScreen({Key? key}) : super(key: key);

  Widget build(BuildContext context) {
    final BookingController controller = Get.put(BookingController());
    final Size size = MediaQuery.of(context).size;

    return Scaffold(
      body: Obx(
        () => LoadingOverlay(
          isLoading: controller.isLoading.value,
          color: MyTheme.backgroundColor,
          progressIndicator: const CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
          ),
          opacity: 1,
          child: SingleChildScrollView(
            child: SafeArea(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  ArrowBack(
                    color: MyTheme.bottomBarColor,
                  ),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Container(
                        margin: const EdgeInsets.only(left: 16, top: 8),
                        child: Text(
                          controller.showtime?.movie?.title ?? "",
                          style: TextStyle(
                              fontSize: 22, fontWeight: FontWeight.bold),
                        ),
                      ),
                      Container(
                        margin: const EdgeInsets.only(top: 4, left: 16),
                        child: Text(
                          controller.showtime?.theaterName ?? '',
                        ),
                      ),
                    ],
                  ),
                  // seat status bar
                  Padding(
                    padding: const EdgeInsets.only(top: 16),
                    child: Container(
                      width: double.infinity,
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: [
                          buildSeatStatusBar(
                              color: MyTheme.grayColor, content: 'Disabled'),
                          buildSeatStatusBar(
                              color: MyTheme.infoColor, content: 'Available'),
                          buildSeatStatusBar(
                              color: MyTheme.errorColor, content: 'Booked'),
                          buildSeatStatusBar(
                              color: MyTheme.primaryColor, content: 'Selected'),
                        ],
                      ),
                    ),
                  ),
                  Padding(
                    padding:
                        EdgeInsets.symmetric(horizontal: 8.0, vertical: 8.0),
                    child: Container(
                      width: double.infinity,
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: List.generate(
                            controller.showtime?.room?.numberOfRow ?? 0,
                            (int row) => Builder(builder: (context) {
                                  return Container(
                                    margin: EdgeInsets.only(top: 8),
                                    child: Row(
                                        mainAxisAlignment:
                                            MainAxisAlignment.spaceBetween,
                                        children: List.generate(
                                            controller.showtime?.room
                                                    ?.numberOfColumn ??
                                                0, (int column) {
                                          var t = controller.tickets.where(
                                              (ticket) =>
                                                  ticket.seat!.rowPos ==
                                                      row + 1 &&
                                                  ticket.seat!.columnPos ==
                                                      column + 1);
                                          if (t.isNotEmpty) {
                                            return ToggleButton(
                                              child: Text(
                                                String.fromCharCode(65 + row) +
                                                    (column + 1).toString(),
                                                style: TextStyle(
                                                    color: Colors.white),
                                              ),
                                              isAvailable: true,
                                              ticket: t.first,
                                            );
                                          } else {
                                            return ToggleButton(
                                              child: Text(
                                                String.fromCharCode(65 + row) +
                                                    (column + 1).toString(),
                                                style: TextStyle(
                                                    color: Colors.white),
                                              ),
                                              isAvailable: false,
                                              ticket: null,
                                            );
                                          }
                                        }).toList()),
                                  );
                                })).reversed.toList(),
                      ),
                    ),
                  ),
                  Container(
                    alignment: Alignment.center,
                    child: const Text(
                      'Screen',
                    ),
                  ),
                  Image.asset("assets/images/screen.png"),
                  Padding(
                    padding:
                        const EdgeInsets.only(left: 24, right: 24, bottom: 16),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Total Price',
                            ),
                            Text(
                              controller.total.value.toString() + " VND",
                              style: TextStyle(
                                  fontSize: 18, fontWeight: FontWeight.w600),
                            )
                          ],
                        ),
                        GestureDetector(
                          onTap: () {
                            Get.toNamed("/checkout", arguments: {
                              'showtime': controller.showtime,
                              'tickets': controller.selected,
                              'total': controller.total.value
                            });
                          },
                          child: Container(
                            height: size.height / 16,
                            width: size.width / 3,
                            alignment: Alignment.center,
                            decoration: BoxDecoration(
                                color: MyTheme.primaryColor,
                                borderRadius: BorderRadius.circular(16)),
                            child: const Text(
                              'Book Ticket',
                              style: TextStyle(
                                  fontWeight: FontWeight.w500,
                                  color: Colors.white),
                            ),
                          ),
                        )
                      ],
                    ),
                  )
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Row buildSeatStatusBar({required Color color, required String content}) {
    return Row(
      children: [
        Container(
          height: 16,
          width: 16,
          decoration: BoxDecoration(
              color: color, borderRadius: BorderRadius.circular(4)),
        ),
        Padding(
          padding: EdgeInsets.only(left: 8.0),
          child: Text(
            content,
            style: TextStyle(fontSize: 12),
          ),
        ),
      ],
    );
  }
}

class ToggleButton extends StatefulWidget {
  const ToggleButton(
      {Key? key, required this.child, required this.isAvailable, this.ticket})
      : super(key: key);
  final Widget child;
  final bool isAvailable;
  final TicketModel? ticket;

  @override
  State<ToggleButton> createState() => _ToggleButtonState();
}

class _ToggleButtonState extends State<ToggleButton> {
  @override
  Widget build(BuildContext context) {
    return SizedBox(
        height: 32,
        width: 32,
        child: GestureDetector(
          onTap: () {
            if (widget.isAvailable) {
              BookingController.instance.setSelected(widget.ticket);
            }
          },
          child: Container(
            alignment: Alignment.center,
            decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(4),
                color: !widget.isAvailable
                    ? MyTheme.grayColor
                    : widget.ticket!.statusName == 'Sold'
                        ? MyTheme.errorColor
                        : !BookingController.instance.selected
                                .contains(widget.ticket)
                            ? MyTheme.infoColor
                            : MyTheme.primaryColor),
            child: widget.child,
          ),
        ));
  }
}
