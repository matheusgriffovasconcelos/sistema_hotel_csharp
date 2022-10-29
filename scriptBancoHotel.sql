--TRABALHO EM GRUPO
--INTEGRANTES: MATHEUS VASCONCELOS, AYRTON, EDIGAR

--Tabela de CLIENTES
CREATE TABLE cliente
(
rg NUMERIC NOT NULL,
    nome VARCHAR(40) NOT NULL,
sexo CHAR(1) NOT NULL,
telefone NUMERIC(11,0),
PRIMARY KEY (rg)
) WITHOUT OIDS;

-- Tabela TIPO_QUARTO
CREATE TABLE tipo_quarto
(
id_tipo SERIAL NOT NULL,
descricao VARCHAR(40) NOT NULL,
valor NUMERIC(9,2) NOT NULL,
PRIMARY KEY (id_tipo)
) WITHOUT OIDS;


-- Tabela QUARTO
CREATE TABLE quarto
(
num_quarto INTEGER NOT NULL,
andar CHAR(10),
id_tipo INTEGER NOT NULL,
status CHAR(01) NOT NULL DEFAULT 'D',
PRIMARY KEY (num_quarto),
FOREIGN KEY (id_tipo)
REFERENCES tipo_quarto (id_tipo)
ON UPDATE RESTRICT ON DELETE RESTRICT
) WITHOUT OIDS;


-- Tabela SERVIÇO
CREATE TABLE servico
(
id_servico SERIAL NOT NULL,
descricao VARCHAR(60) NOT NULL,
valor NUMERIC(9,2) NOT NULL,
PRIMARY KEY (id_servico)
) WITHOUT OIDS;


-- Tabela RESERVA
CREATE TABLE reserva
(
id_reserva SERIAL NOT NULL,
rg NUMERIC NOT NULL,
num_quarto INTEGER NOT NULL,
dt_reserva DATE NOT NULL,
qtd_dias INTEGER NOT NULL,
data_entrada DATE NOT NULL,
    status CHAR(1) NOT NULL DEFAULT 'A',
 PRIMARY KEY (id_reserva),
 FOREIGN KEY (rg) REFERENCES cliente (rg)
 ON UPDATE RESTRICT ON DELETE RESTRICT,
 FOREIGN KEY (num_quarto)
 REFERENCES quarto (num_quarto)
 ON UPDATE RESTRICT ON DELETE RESTRICT
) WITHOUT OIDS;


--Tabela HOSPEDAGEM
CREATE TABLE hospedagem
(
    id_hospedagem SERIAL NOT NULL,
    rg NUMERIC NOT NULL,
    num_quarto INTEGER NOT NULL,
    data_entrada DATE NOT NULL,
    data_saida DATE,
    status CHAR(1) NOT NULL,
 PRIMARY KEY (id_hospedagem),
 FOREIGN KEY (rg) REFERENCES cliente (rg)
 ON UPDATE RESTRICT ON DELETE RESTRICT,
 FOREIGN KEY (num_quarto)
 REFERENCES quarto (num_quarto)
 ON UPDATE RESTRICT ON DELETE RESTRICT
) WITHOUT OIDS;


--Tabela ATENDIMENTO
CREATE TABLE atendimento
(
    id_atendimento SERIAL NOT NULL,
    id_servico INTEGER NOT NULL,
    id_hospedagem INTEGER NOT NULL,
 PRIMARY KEY (id_atendimento),
 FOREIGN KEY (id_servico)
 REFERENCES servico (id_servico)
 ON UPDATE RESTRICT ON DELETE RESTRICT,
 FOREIGN KEY (id_hospedagem)
 REFERENCES hospedagem (id_hospedagem)
 ON UPDATE RESTRICT ON DELETE RESTRICT
) WITHOUT OIDS;



-- Comando de criação da função adicionaHospedagem
CREATE OR REPLACE FUNCTION adicionaHospedagem(rg_cliente numeric, numero_quarto
int) RETURNS void AS
$$
  begin
    perform * from cliente where
     rg = rg_cliente;
    if found then
      perform * from quarto where
      upper(status) = 'D' and
      num_quarto = numero_quarto;
	  
	if found then
        insert into hospedagem
         values (default, rg_cliente,
            numero_quarto, current_date,
            null, 'A');
        update quarto set status = 'O' where
         num_quarto = numero_quarto;
        RAISE NOTICE 'Hospedagem
         realizada com sucesso!';
      else
        RAISE EXCEPTION
          'Quarto indisponivel para hospedagem!';  
      end if;
    else
      RAISE EXCEPTION
       'Cliente nao consta no cadastro!';      
    end if;    
  end;
$$
LANGUAGE plpgsql SECURITY DEFINER;



-- Comando de criação da função adicionaReserva

CREATE OR REPLACE FUNCTION adicionaReserva
 (rg_cliente numeric, numero_quarto int, dias int,
    data_entrada date) RETURNS void AS
$$
  begin
    perform * from cliente where rg = rg_cliente;
    if found then
      perform * from quarto where
      upper(status) = 'D' and
      num_quarto = numero_quarto;
      if found then
        insert into reserva values
        (default, rg_cliente, numero_quarto,
          current_date, dias, data_entrada, 'A');
        update quarto set status = 'R' where
        num_quarto = numero_quarto;
        RAISE NOTICE 'Reserva realizada
         com sucesso!';
      else
        RAISE EXCEPTION 'Quarto indisponivel
           para reserva!'; 
      end if;
    else
      RAISE EXCEPTION 'Cliente nao
          consta no cadastro!';     
    end if;   
  end;
$$
LANGUAGE plpgsql SECURITY DEFINER;


--  Comando de criação da função realizaPedidos

CREATE OR REPLACE FUNCTION realizaPedido(hosp int, serv int) RETURNS void AS
$$
 begin
    perform * from hospedagem where
    upper(status) = 'A' and id_hospedagem = hosp;
    if found then
      perform * from servico where
       id_servico = serv;
      if found then
        insert into atendimento values
         (default, serv, hosp);
        RAISE NOTICE 'Pedido realizado
          com sucesso!';
      else
        RAISE EXCEPTION 'Servico
          indisponivel!';  
      end if;
    else
      RAISE EXCEPTION 'Hospedagem
       nao consta no cadastro ou ja
       foi desativada!';      
    end if;    
  end;
$$
LANGUAGE plpgsql SECURITY DEFINER;


-- Criação da visão para consultar o nome e o sexo dos clientes

CREATE VIEW listaClientes (nome_cliente,sexo) AS
SELECT nome, sexo FROM cliente;

-- . Criação dos papéis (roles) gerente, atendente e estagiário

CREATE ROLE gerente;
CREATE ROLE atendente;
CREATE ROLE estagiario;


--Revogando a execução das três funções para todos os usuários

REVOKE ALL ON FUNCTION adicionaReserva(numeric,int,int,date) FROM PUBLIC;
REVOKE ALL ON FUNCTION adicionaHospedagem(numeric,int) FROM PUBLIC;
REVOKE ALL ON FUNCTION realizaPedido(int,int) FROM PUBLIC;


--Concedendo permissão para o role gerente acessar todas as tabelas e
--conceder permissões para outros usuários
GRANT SELECT, INSERT ON cliente, reserva, hospedagem, quarto, tipo_quarto,
atendimento, servico, listaClientes TO gerente WITH GRANT OPTION;

--Concedendo permissão para o role gerente para acessar a função
--adicionaHospedagem
GRANT EXECUTE ON FUNCTION adicionaHospedagem(numeric,int) TO gerente;

--Concedendo permissão para o role gerente para acessar a função
--adicionaReserva
GRANT EXECUTE ON FUNCTION adicionaReserva(numeric,int,int,date) TO gerente;

--Concedendo permissão para o role gerente para acessar a função
--realizarPedido
GRANT EXECUTE ON FUNCTION realizaPedido(int,int) TO gerente;

--25. Concedendo permissão para o role gerente para acessar a view listaClientes
GRANT SELECT ON listaClientes TO gerente;



--26. Concedendo permissão para o role atendente para acessar a função
--adicionaHospedagem
GRANT EXECUTE ON FUNCTION adicionaHospedagem(numeric,int) TO atendente;

--27. Concedendo permissão para o role atendente para acessar a função
--adicionaReserva
GRANT EXECUTE ON FUNCTION adicionaReserva(numeric,int,int,date) TO atendente;

--28. Concedendo permissão para o role atendente para acessar a função
--realizaPedidos
GRANT EXECUTE ON FUNCTION
realizaPedido(int,int) TO atendente;

--29. Concedendo permissão para o role estagiário acessar a visão listaCliente
GRANT SELECT ON listaClientes TO estagiario;

--30. Criação de usuário com papel de gerente
CREATE ROLE tony LOGIN PASSWORD '111' IN ROLE gerente;

--31. Criação de usuário com papel de atendente
CREATE ROLE maria LOGIN PASSWORD '222' IN ROLE atendente;

--32. Criação de usuário com papel de atendente
CREATE ROLE vitoria LOGIN PASSWORD '333' IN ROLE estagiario;


--criação de uma tabela para armazenar as informações de LOG. Tabela reserva_auditoria


CREATE TABLE reserva_auditoria
(
log_id SERIAL NOT NULL,
id_reserva SERIAL,
data_criacao TEXT NOT NULL,
operacao_realizada CHARACTER VARYING,

CONSTRAINT PK_reserva_auditoria
  PRIMARY KEY (id_reserva),
CONSTRAINT FK_reserva_auditoria_reserva
  FOREIGN KEY (id_reserva)
  REFERENCES reserva(id_reserva)

);

--Função para validar o tipo de operação realizada na tabela reserva(no caso, insert) através de outra função chamada adicionaReserva()

CREATE OR REPLACE FUNCTION reserva_log_reserva()
RETURNS trigger AS $BODY$
BEGIN
IF (TG_OP = 'INSERT') THEN
INSERT INTO reserva_auditoria (id_reserva, data_criacao, operacao_realizada)
VALUES (new.id_reserva, current_timestamp, ' Operação de inserção.
A linha de código ' || NEW.id_reserva|| 'foi inserido');
RETURN NEW;
END IF;
RETURN NULL;
END;
$BODY$ LANGUAGE plpgsql;


CREATE TRIGGER trigger_log_nova_reserva
AFTER INSERT OR UPDATE OR DELETE ON reserva
FOR EACH ROW
EXECUTE PROCEDURE reserva_log_reserva();

