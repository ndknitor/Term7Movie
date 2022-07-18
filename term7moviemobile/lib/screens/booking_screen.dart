import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/booking_controller.dart';
import 'package:term7moviemobile/models/seat_model.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';

class BookingScreen extends StatefulWidget {
  const BookingScreen({Key? key}) : super(key: key);

  @override
  State<BookingScreen> createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  @override
  void initState() {
    super.initState();
    BookingController.instance.fetchData();
  }

  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;

    return Scaffold(
      body: Obx(
        () => LoadingOverlay(
          isLoading: BookingController.instance.isLoading.value,
          color: MyTheme.backgroundColor,
          progressIndicator: const CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
          ),
          opacity: 1,
          child: RefreshIndicator(
            color: MyTheme.primaryColor,
            onRefresh: () async {
              BookingController.instance.fetchData();
            },
            child: ListView(
              children: [
                SafeArea(
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
                              BookingController
                                      .instance.showtime?.movie?.title ??
                                  "",
                              style: TextStyle(
                                  fontSize: 22, fontWeight: FontWeight.bold),
                            ),
                          ),
                          Container(
                            margin: const EdgeInsets.only(top: 4, left: 16),
                            child: Text(
                              BookingController
                                      .instance.showtime?.theaterName ??
                                  '',
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
                                  color: SvgPicture.string(Constants.svgSeatVip
                                      .replaceAll('#FBFBFB', '#D8D8D8')),
                                  content: 'Disabled'),
                              buildSeatStatusBar(
                                  color: SvgPicture.string(Constants.svgSeatVip),
                                  content: 'Available'),
                              buildSeatStatusBar(
                                  color: SvgPicture.string(Constants.svgSeatVip
                                      .replaceAll('#FBFBFB', '#FFBC99')
                                      .replaceAll('#9A9FA5', '#FF4842')),
                                  content: 'Booked'),
                              buildSeatStatusBar(
                                  color: SvgPicture.string(Constants.svgSeatVip
                                      .replaceAll('#FBFBFB', '#CABDFF')
                                      .replaceAll('#9A9FA5', '#6346FA')),
                                  content: 'Selected'),
                            ],
                          ),
                        ),
                      ),
                      //seat type bar
                      Padding(
                        padding: const EdgeInsets.only(top: 16),
                        child: Container(
                          width: double.infinity,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                            children: [
                              buildSeatStatusBar(
                                  color: SvgPicture.string(Constants.svgSeatNormal),
                                  content: 'Normal'),
                              buildSeatStatusBar(
                                  color: SvgPicture.string(Constants.svgSeatVip),
                                  content: 'Vip'),
                            ],
                          ),
                        ),
                      ),
                      Container(
                        alignment: Alignment.center,
                        padding: EdgeInsets.symmetric(
                            horizontal: 8.0, vertical: 8.0),
                        child: SingleChildScrollView(
                          physics: AlwaysScrollableScrollPhysics(),
                            scrollDirection: Axis.horizontal,
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              crossAxisAlignment: CrossAxisAlignment.end,
                              children: [
                                Column(
                                  mainAxisAlignment:
                                  MainAxisAlignment.spaceBetween,
                                  children: List.generate(
                                      BookingController.instance.showtime?.room
                                          ?.numberOfRow ??
                                          0,
                                          (int row) => Builder(builder: (context) {
                                        return Container(
                                          height: 24,
                                          width: 24,
                                          margin: EdgeInsets.only(top: 8, bottom: 2),
                                          child: Text(
                                            String.fromCharCode(65 + row),
                                            style: TextStyle(fontSize: 16),
                                          ),
                                        );
                                      })).reversed.toList(),
                                ),
                                Column(
                                  children: [
                                    Row(
                                      children: List.generate(
                                          BookingController.instance.showtime
                                              ?.room?.numberOfColumn ??
                                              0,
                                              (int column) =>
                                              Builder(builder: (context) {
                                                return Container(
                                                  height: 24,
                                                  width: 24,
                                                  margin: EdgeInsets.only(
                                                      left: 6, right: 4, top: 8),
                                                  child: Text(
                                                    (column + 1).toString(),
                                                    style:
                                                    TextStyle(fontSize: 16),
                                                    textAlign: TextAlign.center,
                                                  ),
                                                );
                                              })).toList(),
                                    ),
                                    Column(
                                      children: List.generate(
                                          BookingController.instance.showtime
                                              ?.room?.numberOfRow ??
                                              0,
                                              (int row) =>
                                              Builder(builder: (context) {
                                                return Container(
                                                  child: Row(
                                                      mainAxisAlignment:
                                                      MainAxisAlignment
                                                          .spaceBetween,
                                                      children: List.generate(
                                                          BookingController
                                                              .instance
                                                              .showtime
                                                              ?.room
                                                              ?.numberOfColumn ??
                                                              0, (int column) {
                                                        var s = BookingController
                                                            .instance
                                                            .showtime!
                                                            .room!
                                                            .seatList!
                                                            .where((seat) =>
                                                        seat.rowPos ==
                                                            row + 1 &&
                                                            seat.columnPos ==
                                                                column + 1);
                                                        if (s.isNotEmpty) {
                                                          var t = BookingController
                                                              .instance.tickets
                                                              .where((ticket) =>
                                                          ticket.seat!
                                                              .rowPos ==
                                                              s.first
                                                                  .rowPos &&
                                                              ticket.seat!
                                                                  .columnPos ==
                                                                  s.first
                                                                      .columnPos);
                                                          if (t.isNotEmpty) {
                                                            if (t.first.lockedTime ==
                                                                null ||
                                                                (DateTime.parse(t
                                                                    .first
                                                                    .lockedTime!)
                                                                    .add(Duration(
                                                                    hours:
                                                                    7))
                                                                    .isBefore(
                                                                    DateTime
                                                                        .now()) &&
                                                                    t.first.statusName !=
                                                                        'Sold')) {
                                                              return ToggleButton(
                                                                isAvailable: true,
                                                                ticket: t.first,
                                                                isSold: false,
                                                                seat: s.first,
                                                              );
                                                            } else {
                                                              return ToggleButton(
                                                                isAvailable: true,
                                                                ticket: t.first,
                                                                isSold: true,
                                                                seat: s.first,
                                                              );
                                                            }
                                                          } else {
                                                            return ToggleButton(
                                                              isAvailable: false,
                                                              isSold: false,
                                                              seat: s.first,
                                                            );
                                                          }
                                                        } else {
                                                          return Container(
                                                            height: 24,
                                                            width: 24,
                                                            margin:
                                                            const EdgeInsets
                                                                .all(5.0),
                                                          );
                                                        }
                                                      }).toList()),
                                                );
                                              })).reversed.toList(),
                                    ),
                                  ],
                                ),
                              ],
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
                        padding: const EdgeInsets.only(
                            left: 24, right: 24, bottom: 16),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  'Your Seats',
                                ),
                                Text(
                                  BookingController.instance.selected.map((e) => e.seat!.name)
                                      .toList()
                                      .join(", "),
                                  style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.w600),
                                ),
                                SizedBox(height: 8,),
                                Text(
                                  'Total Price',
                                ),
                                Text(
                                  BookingController.instance.total.value
                                      .toStringAsFixed(0) +
                                      " VND",
                                  style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.w600),
                                ),
                              ],
                            ),
                            GestureDetector(
                              onTap: () {
                                Get.toNamed("/checkout", arguments: {
                                  'showtime':
                                      BookingController.instance.showtime,
                                  'tickets':
                                      BookingController.instance.selected,
                                  'total':
                                      BookingController.instance.total.value
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
              ],
            ),
          ),
        ),
      ),
    );
  }

  Row buildSeatStatusBar({required var color, required String content}) {
    return Row(
      children: [
        Container(
          alignment: Alignment.center,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(4),
          ),
          child: color,
          height: 20,
          width: 20,
        ),
        Padding(
          padding: EdgeInsets.only(left: 2.0),
          child: Text(
            content,
            style: TextStyle(fontSize: 12),
          ),
        ),
      ],
    );
  }
}

class ToggleButton extends StatelessWidget {
  final bool isAvailable;
  final bool isSold;
  final TicketModel? ticket;
  final SeatModel seat;
  const ToggleButton(
      {Key? key, required this.isAvailable, this.ticket, required this.isSold, required this.seat})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
        padding: const EdgeInsets.all(5.0),
        child: GestureDetector(
            onTap: () {
              if (ticket!.statusName != 'Sold') {
                BookingController.instance.setSelected(ticket);
              }
            },
            child: !isAvailable
                ? Container(
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(4),
                    ),
                    child: seat.seatTypeId == 2 ?  SvgPicture.string(
                      Constants.svgSeatVip.replaceAll('#FBFBFB', '#D8D8D8'),
                      height: 24,
                      width: 24,
                    ) : SvgPicture.string(
                      Constants.svgSeatNormal.replaceAll('#FBFBFB', '#D8D8D8'),
                      height: 24,
                      width: 24,
                    ),
                  )
                : isSold
                    ? Container(
                        alignment: Alignment.center,
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(4),
                        ),
                        child: seat.seatTypeId == 2 ?  SvgPicture.string(
                          Constants.svgSeatVip
                              .replaceAll('#FBFBFB', '#FFBC99')
                              .replaceAll('#9A9FA5', '#FF4842'),
                          height: 24,
                          width: 24,
                        ) : SvgPicture.string(
                          Constants.svgSeatNormal.replaceAll('#FBFBFB', '#FFBC99').replaceAll('#9A9FA5', '#FF4842'),
                          height: 24,
                          width: 24,
                        ),
                      )
                    : Obx(() {
                        var color = seat.seatTypeId == 2 ? Constants.svgSeatVip : Constants.svgSeatNormal;
                        // if (!BookingController.instance.selected.contains(ticket)) {
                        //   color = Constants.svgCode.replaceAll('#FBFBFB', '#74CAFF').replaceAll('#9A9FA5', '#0068FF');
                        // }
                        if (BookingController.instance.selected
                            .contains(ticket)) {
                          color = Constants.svgSeatVip
                              .replaceAll('#FBFBFB', '#CABDFF')
                              .replaceAll('#9A9FA5', '#6346FA');
                        }
                        return AnimatedContainer(
                          duration: const Duration(milliseconds: 200),
                          alignment: Alignment.center,
                          decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(4)),
                          child: SvgPicture.string(
                            color,
                            height: 24,
                            width: 24,
                          ),
                        );
                      })));
  }
}
