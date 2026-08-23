FROM docker.io/archlinux/archlinux:latest

ARG TEST_USER
ARG TEST_PASSWORD

ENV TEST_PASSWORD=${TEST_PASSWORD}
ENV PATH="/usr/local/bin:${PATH}"

RUN pacman-key --init \
    && pacman -Syu --noconfirm sudo \
    && useradd -m -s /bin/bash -G wheel "$TEST_USER" \
    && echo "$TEST_USER:$TEST_PASSWORD" | chpasswd \
    && echo '%wheel ALL=(ALL:ALL) ALL' > /etc/sudoers.d/wheel \
    && chmod 440 /etc/sudoers.d/wheel

COPY Resources/sudo-wrapper.sh /usr/local/bin/sudo

RUN chmod 755 /usr/local/bin/sudo

USER ${TEST_USER}
